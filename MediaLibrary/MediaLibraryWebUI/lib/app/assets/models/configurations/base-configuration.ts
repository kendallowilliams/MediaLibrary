import BaseClass from "../base-class";

export default class BaseConfiguration extends BaseClass {
    constructor(private controller: string) {
        super();
    }

    protected update<T>(data: T, callback: () => void = () => null): void {
        let url = new String('/').concat(this.controller).concat('/UpdateConfiguration');

        $.post(url, data, callback);
    }
}