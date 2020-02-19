import BaseConfiguration from './base-configuration'
import * as Enums from '../../enums/enums'

export default class MediaLibraryConfiguration extends BaseConfiguration {
    private selectedMediaPage: Enums.MediaPages;

    constructor() {
        super();
    }
}
