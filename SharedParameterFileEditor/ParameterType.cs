﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedParameterFileEditor
{
    enum ParameterType
    {
        //common
        TEXT,
        HVAC_SPECIFIC_HEAT,
        //structural
        ACCELERATION,
        AREA_FORCE,
        AREA_SPRING_COEFFICIENT,
        BAR_DIAMETER,
        CRACK_WIDTH,
        //TODO: handle this DISPLACEMENT/DEFLECTION,
        FORCE,
        LINEAR_FORCE,
        LINEAR_MOMENT,
        LINEAR_SPRING_COEFFICIENT,
        MASS,
        MASS_PER_UNIT_AREA,
        MASS_PER_UNIT_LENGTH,
        MOMENT,
        MOMENT_OF_INERTIA,
        PERIOD,
        POINT_SPRING_COEFFICIENT,
        PULSATION,
        REINFORCEMENT_AREA,
        REINFORCEMENT_AREA_PER_UNIT_LENGTH,
        REINFORCEMENT_COVER,
        REINFORCEMENT_LENGTH,
        REINFORCEMENT_SPACING,
        REINFORCEMENT_VOLUME,
        ROTATION,
        ROTATIONAL_LINEAR_SPRING_COEFFICIENT,
        ROTATIONAL_POINT_SPRING_COEFFICIENT,
        SECTION_AREA,
        SECTION_DIMENSION,
        SECTION_MODULUS,
        SECTION_PROPERTY,
        STRESS,
        STRUCTURAL_FREQUENCY,
        SURFACE_AREA,
        THERMAL_EXPANSION_COEFFICIENT,
        UNIT_WEIGHT,
        VELOCITY,
        WARPING_CONSTANT,
        WEIGHT,
        WEIGHT_PER_UNIT_LENGTH,
        //piping
        PIPE_DIMENSION,
        PIPING_DENSITY,
        PIPING_SLOPE,
        PIPING_TEMPERATURE,
        //hvac
        HVAC_AIR_FLOW,
        HVAC_AIRFLOW_DENSITY,
        HVAC_AIRFLOW_DIVIDED_BY_COOLING_LOAD,
        HVAC_AIRFLOW_DIVIDED_BY_VOLUME,
        HVAC_ANGULAR_SPEED,
        HVAC_AREA_DIVIDED_BY_COOLING_LOAD,
        HVAC_AREA_DIVIDED_BY_HEATING_LOAD,
        HVAC_COOLING_LOAD,
        HVAC_COOLING_LOAD_DIVIDED_BY_AREA,
        HVAC_COOLING_LOAD_DIVIDED_BY_VOLUME,
        HVAC_CROSS_SECTION,
        HVAC_DENSITY,
        HVAC_DIFFUSIVITY,
        HVAC_DUCT_INSULATION_THICKNESS,
        HVAC_DUCT_LINING_THICKNESS,
        HVAC_DUCT_SIZE,
        HVAC_FACTOR,
        HVAC_FLOW_PER_POWER,
        HVAC_FRICTION,
        HVAC_HEAT_GAIN,
        HVAC_HEATING_LOAD,
        HVAC_HEATING_LOAD_DIVIDED_BY_AREA,
        HVAC_HEATING_LOAD_DIVIDED_BY_VOLUME,
        HVAC_MASS_PER_TIME,
        HVAC_POWER,
        HVAC_POWER_DENSITY,
        HVAC_POWER_PER_FLOW,
        HVAC_PRESSURE,
        HVAC_ROUGHNESS,
        HVAC_SLOPE,
        HVAC_TEMPERATURE,
        HVAC_TEMPERATURE_DIFFERENCE,
        HVAC_VELOCITY,
        HVAC_VISCOSITY,
        //electrical
        CABLETRAY_SIZE,
        COLOR_TEMPERATURE,
        CONDUIT_SIZE,
        ELECTRICAL_APPARENT_POWER,
        ELECTRICAL_COST_RATE_ENERGY,
        ELECTRICAL_COST_RATE_POWER,
        ELECTRICAL_CURRENT,
        ELECTRICAL_DEMAND_FACTOR,
        ELECTRICAL_EFFICACY,
        ELECTRICAL_FREQUENCY,
        ELECTRICAL_ILLUMINANCE,
        ELECTRICAL_LUMINOUS_FLUX,
        ELECTRICAL_LUMINOUS_INTENSITY,
        ELECTRICAL_POTENTIAL,
        ELECTRICAL_POWER,
        ELECTRICAL_POWER_DENSITY,
        ELECTRICAL_POWER_PER_LENGTH,
        ELECTRICAL_RESISTIVITY,
        ELECTRICAL_TEMPERATURE,
        ELECTRICAL_TEMPERATURE_DIFFERENCE,
        ELECTRICAL_WATTAGE,
        LOADCLASSIFICATION,
        NOOFPOLES,
        WIRE_SIZE,
    }
}