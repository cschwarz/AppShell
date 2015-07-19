function ServiceDispatcher(data) {
    this._services = {};
}

ServiceDispatcher.prototype.initialize = function (services) {    
    var self = this;

    Object.keys(services).forEach(function (serviceName) {
        var service = {};

        services[serviceName].forEach(function (methodName) {
            service[methodName] = function () {
                //return JSON.parse(window.external.Dispatch(serviceName, methodName, JSON.stringify(arguments)));
                //return 5;
                return Native('dispatch', JSON.stringify(arguments));
            };
        });

        self._services[serviceName] = service;
    });

    if (window.serviceDispatcherReady)
        window.serviceDispatcherReady();
};

ServiceDispatcher.prototype.dispatch = function (serviceName) {
    return this._services[serviceName];
};

ServiceDispatcher.prototype.subscribeEvent = function (serviceName, eventName, callback) {
    return window.external.SubscribeEvent(serviceName, eventName, function (e) {
        callback(JSON.parse(e));
    });
};

window.serviceDispatcher = new ServiceDispatcher();