namespace Magentix.Presentation.Services.Common
{
    public class OperationRequest<T>
    {
        private readonly string _expectedEvent;

        public string Data
        {
            get;
            set;
        }

        public T SelectedItem
        {
            get;
            set;
        }

        public OperationRequest(T selectedItem, string expectedEvent)
        {
            this.SelectedItem = selectedItem;
            this._expectedEvent = expectedEvent;
            this.Data = "";
        }

        public string GetExpectedEvent()
        {
            return this._expectedEvent;
        }

        public void Publish(T selectedEntity)
        {
            this.SelectedItem = selectedEntity;
            this.PublishEvent<OperationRequest<T>>(this._expectedEvent);
        }

        public static void Publish(T selectedItem, string requestedEvent, string expectedEvent, string data, bool wait = false)
        {
            OperationRequest<T> operationRequest = new OperationRequest<T>(selectedItem, expectedEvent)
            {
                Data = data
            };
            operationRequest.PublishEvent<OperationRequest<T>>(requestedEvent, wait);
        }
    }
}