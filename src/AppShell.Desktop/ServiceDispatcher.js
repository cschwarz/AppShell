function ServiceDispatcher(data) {
    this._services = {};
}

ServiceDispatcher.prototype.initialize = function (services) {
    var self = this;

    Object.keys(services).forEach(function (serviceName) {
        var service = {};

        services[serviceName].forEach(function (methodName) {
            service[methodName] = function () {
                window.external.Dispatch(serviceName, methodName, JSON.stringify(arguments));
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

window.serviceDispatcher = new ServiceDispatcher();