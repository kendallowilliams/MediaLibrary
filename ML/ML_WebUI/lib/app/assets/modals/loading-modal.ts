import HtmlControls from '../controls/html-controls';

export default {
    showLoading: function (): void {
        var $modal = $(HtmlControls.Modals().LoadingModal),
            processCount = parseInt($modal.attr('data-process-count'));

        if (!isNaN(processCount)) {
            $modal.attr('data-process-count', processCount++);
        } else {
            $modal.attr('data-process-count', 1);
        }

        $modal.modal('show');
    },
    hideLoading: function (): void {
        var $modal = $(HtmlControls.Modals().LoadingModal),
            processCount = parseInt($modal.attr('data-process-count'));

        if (!isNaN(processCount)) {
            $modal.attr('data-process-count', processCount--);

            if (processCount == 0) /*then*/ $modal.modal('hide');
        } else {
            $modal.attr('data-process-count', 0);
        }
    }
};