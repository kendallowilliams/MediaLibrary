import BaseClass from "../base-class";
import { getUrl } from "../../utilities/http";

export default abstract class BaseConfiguration extends BaseClass {
    constructor(private controller: string) {
        super();
    }

    protected update<T>(data: T, callback: () => void = () => null): void {
        let url = this.controller.concat('/UpdateConfiguration');

        $.post(url, data, callback);
    }
}