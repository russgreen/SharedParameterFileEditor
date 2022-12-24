using CommunityToolkit.Mvvm.Messaging.Messages;
using SharedParametersFile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedParameterFileEditor.Messages
{
    class ListOfGroupsMessage : ValueChangedMessage<List<GroupModel>>
    {
        public ListOfGroupsMessage(List<GroupModel> value) : base(value)
        {
        }
    }
}
