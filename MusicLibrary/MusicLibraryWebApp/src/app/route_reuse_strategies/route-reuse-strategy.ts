import { RouteReuseStrategy, ActivatedRouteSnapshot, DetachedRouteHandle } from '@angular/router';

export class MlRouteReuseStrategy implements RouteReuseStrategy {
  _cacheRouters: { [key: string]: any } = {};
  routesToIgnore: string[] = ['podcasts'];

  shouldDetach(route: ActivatedRouteSnapshot): boolean {
    return this.routesToIgnore.indexOf(route.routeConfig.path) === -1;
  }

  store(route: ActivatedRouteSnapshot, handle: DetachedRouteHandle): void {
    this._cacheRouters[route.routeConfig.path] = {
      snapshot: route,
      handle: handle
    };
  }

  shouldAttach(route: ActivatedRouteSnapshot): boolean {
    return !!route.routeConfig && !!this._cacheRouters[route.routeConfig.path];
  }

  retrieve(route: ActivatedRouteSnapshot): DetachedRouteHandle {
    if (!route.routeConfig || !this._cacheRouters[route.routeConfig.path]) {
      return null;
    }
    return this._cacheRouters[route.routeConfig.path].handle;
  }

  shouldReuseRoute(future: ActivatedRouteSnapshot, curr: ActivatedRouteSnapshot): boolean {
    if (this.routesToIgnore.indexOf(future.routeConfig && future.routeConfig.path) > 1) {
      return false;
    } else {
      return future.routeConfig === curr.routeConfig;
    }
  }
}
