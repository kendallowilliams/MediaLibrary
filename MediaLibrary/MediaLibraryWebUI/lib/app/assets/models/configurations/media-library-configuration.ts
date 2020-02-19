import BaseConfiguration from './base-configuration'
import * as Enums from '../../enums/enums'

export default class MediaLibraryConfiguration extends BaseConfiguration {
    private _selectedMediaPage: Enums.MediaPages;

    get selectedMediaPage(): Enums.MediaPages {
        return this._selectedMediaPage;
    }

    set selectedMediaPage(value) {
        this._selectedMediaPage = value;
    }

    constructor(json: any) {
        super();
    }
}
