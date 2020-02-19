import HtmlControls from '../controls/html-controls';

export default {
    showLoading: function (): void {
        var $modal = $(HtmlControls.Modals.LoadingModal),
            processCount = parseInt($modal.attr('data-process-count'));

        if (!isNaN(processCount)) {
            $modal.attr('data-process-count', processCount++);
        } else {
            $modal.attr('data-process-count', 1);
        }

        $(new String('[data-target="#').concat($(HtmlControls.Modals.LoadingModal).attr('id')).concat('"]')).trigger('click');
    },
    hideLoading: function (): void {
        var $modal = $(HtmlControls.Modals.LoadingModal),
            processCount = parseInt($modal.attr('data-process-count'));

        if (!isNaN(processCount)) {
            $modal.attr('data-process-count', processCount--);

            if (processCount == 0) /*then*/ $(HtmlControls.Modals.LoadingModal).find('[data-dismiss="modal"]').trigger('click');
        } else {
            $modal.attr('data-process-count', 0);
        }
    }
};