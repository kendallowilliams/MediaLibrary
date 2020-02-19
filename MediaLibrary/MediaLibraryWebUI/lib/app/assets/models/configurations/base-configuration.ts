import BaseClass from "../base-class";

export default class BaseConfiguration extends BaseClass {
    private _scrollTop: number;
    
    get scrollTop(): number {
        return this._scrollTop;
    }

    set scrollTop(value: number) {
        this._scrollTop = value;
    }

    constructor() {
        super();
    }
}