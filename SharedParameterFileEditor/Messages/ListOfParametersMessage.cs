using CommunityToolkit.Mvvm.Messaging.Messages;
using SharedParametersFile.Models;

namespace SharedParameterFileEditor.Messages
{
    internal class ListOfParametersMessage : ValueChangedMessage<List<ParameterModel>>
    {
        public ListOfParametersMessage(List<ParameterModel> value) : base(value)
        {
        }
    }
}
