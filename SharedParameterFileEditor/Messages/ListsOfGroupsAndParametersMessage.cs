using CommunityToolkit.Mvvm.Messaging.Messages;
using SharedParameterFileEditor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedParameterFileEditor.Messages
{
    class ListsOfGroupsAndParametersMessage : ValueChangedMessage<GroupsAndParametersModel>
    {
        public ListsOfGroupsAndParametersMessage(GroupsAndParametersModel value) : base(value)
        {
        }
    }
}
