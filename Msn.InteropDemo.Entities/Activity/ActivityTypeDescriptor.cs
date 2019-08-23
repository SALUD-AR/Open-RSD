
namespace Msn.InteropDemo.Entities.Activity
{
    public enum ActivityType
    {
        GET_PACIENTE_EN_DB_LOCAL = 1,
        SEARCH_PACIENTES_COINCIDENTES_DB_LOCAL,
        CREATE_PACIENTE_EN_DB_LOCAL,
        GET_PACIENTE_EN_BUS_BY_IDENTIFIER,
        GET_PACIENTE_EN_BUS_MATCH,
        FEDERAR_PACIENTE_EN_BUS,
        SNOWSTORM_FIND_CONCEPTS
    }

    public class ActivityTypeDescriptor : Core.EntityDescriptor
    {
    }
}
