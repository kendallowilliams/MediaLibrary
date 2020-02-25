import BaseClass from "../base-class";

export default abstract class BaseConfiguration<T> extends BaseClass {
    public properties: T;

    constructor(private controller: string) {
        super();
    }

    protected update<T>(data: T, callback: () => void = () => null): void {
        const url = this.controller.concat('/UpdateConfiguration');

        $.post(url, data, callback);
    }

    refresh(callback: () => void = () => null): void {
        const url = this.controller.concat('/').concat(this.controller).concat('Configuration'),
             success: (data) => void = (data) => {
                 this.properties = data;
                 callback();
             };

        $.get(url, success);
    }
}