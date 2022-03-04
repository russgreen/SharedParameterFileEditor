using SharedParametersFile.Models;
using System.Collections.Generic;

namespace SharedParameterFileEditor.Requesters
{
    public interface IParameterListRequester
    {
        void ParameterListComplete(List<ParameterModel> parameters, List<GroupModel> groups);
    }
}
