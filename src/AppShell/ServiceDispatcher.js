﻿function ServiceDispatcher(data) {
    this._services = {};
    this._dispatchCallbacks = {};
    this._eventCallbacks = {};

    this._currentCallbackId = 0;    
}

ServiceDispatcher.prototype.initialize = function (services) {
    var self = this;
    
    Object.keys(services).forEach(function (serviceName) {
        var service = {};
        
        services[serviceName].forEach(function (methodName) {
            service[methodName] = function () {
                var parameters = [];
                var callbackId;
                
                for (var i = 0; i < arguments.length; i++) {
                    if (typeof arguments[i] === 'function') {
                        callbackId = (++self._currentCallbackId).toString();
                        self._dispatchCallbacks[callbackId] = arguments[i];
                    } else {
                        parameters.push(arguments[i]);
                    }
                }
                
                if (window.Native)
                    window.Native('dispatch', JSON.stringify({ serviceName: serviceName, instanceName: this.__instanceName, methodName: methodName, callbackId: callbackId, arguments: parameters }));
                else if (window.external)
                    window.external.Dispatch(serviceName, this.__instanceName, methodName, callbackId, JSON.stringify(parameters));
                
            };
        });

        self._services[serviceName] = service;
    });

    if (window.serviceDispatcherReady)
        window.serviceDispatcherReady();
};

ServiceDispatcher.prototype.dispatch = function (serviceName, instanceName) {
    var service = this._services[serviceName];
    service.__instanceName = instanceName;
    return service;
};

ServiceDispatcher.prototype._dispatchCallback = function (callbackId, result) {
    if (this._dispatchCallbacks[callbackId]) {
        this._dispatchCallbacks[callbackId](result);
        delete this._dispatchCallbacks[callbackId];
    }
};

ServiceDispatcher.prototype._eventCallback = function (callbackId, result) {
    if (this._eventCallbacks[callbackId]) {
        this._eventCallbacks[callbackId](result);
    }
};

ServiceDispatcher.prototype.subscribeEvent = function (serviceName, eventName, callback) {
    var callbackId = (++this._currentCallbackId).toString();
    this._eventCallbacks[callbackId] = function (e) {
        callback(JSON.parse(e));
    };

    if (window.Native)
        window.Native('subscribeEvent', { serviceName: serviceName, eventName: eventName, callbackId: callbackId });
    else if (window.external)
        window.external.SubscribeEvent(serviceName, eventName, callbackId);

    return callbackId;
};

ServiceDispatcher.prototype.unsubscribeEvent = function (serviceName, callbackId) {
    if (window.Native)
        window.Native('unsubscribeEvent', { serviceName: serviceName, callbackId: callbackId });
    else if (window.external)
        window.external.UnsubscribeEvent(serviceName, callbackId);
};

window.serviceDispatcher = new ServiceDispatcher();