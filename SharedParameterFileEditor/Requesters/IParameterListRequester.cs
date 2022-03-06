using SharedParametersFile.Models;

namespace SharedParameterFileEditor.Requesters;

public interface IParameterListRequester
{
    void ParameterListComplete(List<ParameterModel> parameters, List<GroupModel> groups);
}
