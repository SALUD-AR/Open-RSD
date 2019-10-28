
namespace Msn.InteropDemo.Entities.Activity
{
    public enum ActivityType
    {
        GET_PACIENTE_EN_DB_LOCAL                = 1,
        SEARCH_PACIENTES_COINCIDENTES_DB_LOCAL  = 2,
        CREATE_PACIENTE_EN_DB_LOCAL             = 3,
        GET_PACIENTE_EN_BUS_BY_IDENTIFIER       = 4,
        GET_PACIENTE_EN_BUS_MATCH               = 5,
        FEDERAR_PACIENTE_EN_BUS                 = 6,
        SNOWSTORM_FIND_CONCEPTS                 = 7,
        IMMUNIZATION_POST_VACUNA_NOMIVAC        = 8
    }

    public class ActivityTypeDescriptor : Core.EntityDescriptor
    {
    }
}
