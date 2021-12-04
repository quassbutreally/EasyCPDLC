/*  EASYCPDLC: CPDLC Client for the VATSIM Network
    Copyright (C) 2021 Joshua Seagrave joshseagrave@googlemail.com

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/

namespace EasyCPDLC
{

    public class SimbriefRootobject
    {
        public Fetch fetch { get; set; }
        public Params _params { get; set; }
        public SimbriefGeneral general { get; set; }
        public Origin origin { get; set; }
        public Destination destination { get; set; }
        public Alternate alternate { get; set; }
        public Takeoff_Altn takeoff_altn { get; set; }
        public Enroute_Altn enroute_altn { get; set; }
        public Navlog navlog { get; set; }
        public Etops etops { get; set; }
        public Atc atc { get; set; }
        public Aircraft aircraft { get; set; }
        public Fuel fuel { get; set; }
        public Times times { get; set; }
        public Weights weights { get; set; }
        public Impacts impacts { get; set; }
        public Crew crew { get; set; }
        public Notams notams { get; set; }
        public Weather weather { get; set; }
        public Sigmets sigmets { get; set; }
        public Text text { get; set; }
        public Tracks tracks { get; set; }
        public Database_Updates database_updates { get; set; }
        public Files files { get; set; }
        public Fms_Downloads fms_downloads { get; set; }
        public Images images { get; set; }
        public Links links { get; set; }
        public string vatsim_prefile { get; set; }
        public string ivao_prefile { get; set; }
        public string pilotedge_prefile { get; set; }
        public string poscon_prefile { get; set; }
        public string map_data { get; set; }
        public Api_Params api_params { get; set; }
    }

    public class Fetch
    {
        public string userid { get; set; }
        public Static_Id static_id { get; set; }
        public string status { get; set; }
        public string time { get; set; }
    }

    public class Static_Id
    {
    }

    public class Params
    {
        public string request_id { get; set; }
        public string user_id { get; set; }
        public string time_generated { get; set; }
        public Static_Id1 static_id { get; set; }
        public string ofp_layout { get; set; }
        public string airac { get; set; }
        public string units { get; set; }
    }

    public class Static_Id1
    {
    }

    public class SimbriefGeneral
    {
        public string release { get; set; }
        public string icao_airline { get; set; }
        public string flight_number { get; set; }
        public string is_etops { get; set; }
        public string dx_rmk { get; set; }
        public Sys_Rmk sys_rmk { get; set; }
        public string is_detailed_profile { get; set; }
        public string cruise_profile { get; set; }
        public string climb_profile { get; set; }
        public string descent_profile { get; set; }
        public string alternate_profile { get; set; }
        public string reserve_profile { get; set; }
        public string costindex { get; set; }
        public string cont_rule { get; set; }
        public string initial_altitude { get; set; }
        public string stepclimb_string { get; set; }
        public string avg_temp_dev { get; set; }
        public string avg_tropopause { get; set; }
        public string avg_wind_comp { get; set; }
        public string avg_wind_dir { get; set; }
        public string avg_wind_spd { get; set; }
        public string gc_distance { get; set; }
        public string route_distance { get; set; }
        public string air_distance { get; set; }
        public string total_burn { get; set; }
        public string cruise_tas { get; set; }
        public string cruise_mach { get; set; }
        public string passengers { get; set; }
        public string route { get; set; }
        public string route_ifps { get; set; }
        public string route_navigraph { get; set; }
    }

    public class Sys_Rmk
    {
    }

    public class Origin
    {
        public string icao_code { get; set; }
        public string iata_code { get; set; }
        public Faa_Code faa_code { get; set; }
        public string elevation { get; set; }
        public string pos_lat { get; set; }
        public string pos_long { get; set; }
        public string name { get; set; }
        public string plan_rwy { get; set; }
        public string trans_alt { get; set; }
        public string trans_level { get; set; }
    }

    public class Faa_Code
    {
    }

    public class Destination
    {
        public string icao_code { get; set; }
        public string iata_code { get; set; }
        public Faa_Code1 faa_code { get; set; }
        public string elevation { get; set; }
        public string pos_lat { get; set; }
        public string pos_long { get; set; }
        public string name { get; set; }
        public string plan_rwy { get; set; }
        public string trans_alt { get; set; }
        public string trans_level { get; set; }
    }

    public class Faa_Code1
    {
    }

    public class Alternate
    {
        public string icao_code { get; set; }
        public string iata_code { get; set; }
        public Faa_Code2 faa_code { get; set; }
        public string elevation { get; set; }
        public string pos_lat { get; set; }
        public string pos_long { get; set; }
        public string name { get; set; }
        public string plan_rwy { get; set; }
        public string trans_alt { get; set; }
        public string trans_level { get; set; }
        public string cruise_altitude { get; set; }
        public string distance { get; set; }
        public string gc_distance { get; set; }
        public string air_distance { get; set; }
        public string track_true { get; set; }
        public string track_mag { get; set; }
        public string tas { get; set; }
        public string gs { get; set; }
        public string avg_wind_comp { get; set; }
        public string avg_wind_dir { get; set; }
        public string avg_wind_spd { get; set; }
        public string avg_tropopause { get; set; }
        public string avg_tdv { get; set; }
        public string ete { get; set; }
        public string burn { get; set; }
        public string route { get; set; }
        public string route_ifps { get; set; }
    }

    public class Faa_Code2
    {
    }

    public class Takeoff_Altn
    {
    }

    public class Enroute_Altn
    {
    }

    public class Navlog
    {
        public Fix[] fix { get; set; }
    }

    public class Fix
    {
        public string ident { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public object frequency { get; set; }
        public string pos_lat { get; set; }
        public string pos_long { get; set; }
        public string stage { get; set; }
        public string via_airway { get; set; }
        public string is_sid_star { get; set; }
        public string distance { get; set; }
        public string track_true { get; set; }
        public string track_mag { get; set; }
        public string heading_true { get; set; }
        public string heading_mag { get; set; }
        public string altitude_feet { get; set; }
        public string ind_airspeed { get; set; }
        public string true_airspeed { get; set; }
        public string mach { get; set; }
        public string mach_thousandths { get; set; }
        public string wind_component { get; set; }
        public string groundspeed { get; set; }
        public string time_leg { get; set; }
        public string time_total { get; set; }
        public string fuel_flow { get; set; }
        public string fuel_leg { get; set; }
        public string fuel_totalused { get; set; }
        public string fuel_min_onboard { get; set; }
        public string fuel_plan_onboard { get; set; }
        public string oat { get; set; }
        public string oat_isa_dev { get; set; }
        public string wind_dir { get; set; }
        public string wind_spd { get; set; }
        public string shear { get; set; }
        public string tropopause_feet { get; set; }
        public string ground_height { get; set; }
        public string mora { get; set; }
        public string fir { get; set; }
        public string fir_units { get; set; }
        public string fir_valid_levels { get; set; }
        public Wind_Data wind_data { get; set; }
        public Fir_Crossing fir_crossing { get; set; }
    }

    public class Wind_Data
    {
        public Level[] level { get; set; }
    }

    public class Level
    {
        public string altitude { get; set; }
        public string wind_dir { get; set; }
        public string wind_spd { get; set; }
        public string oat { get; set; }
    }

    public class Fir_Crossing
    {
        public Fir fir { get; set; }
    }

    public class Fir
    {
        public string fir_icao { get; set; }
        public string fir_name { get; set; }
        public string pos_lat_entry { get; set; }
        public string pos_long_entry { get; set; }
    }

    public class Etops
    {
    }

    public class Atc
    {
        public string flightplan_text { get; set; }
        public string route { get; set; }
        public string route_ifps { get; set; }
        public string callsign { get; set; }
        public string initial_spd { get; set; }
        public string initial_spd_unit { get; set; }
        public string initial_alt { get; set; }
        public string initial_alt_unit { get; set; }
        public string section18 { get; set; }
        public string fir_orig { get; set; }
        public string fir_dest { get; set; }
        public string fir_altn { get; set; }
        public Fir_Etops fir_etops { get; set; }
        public string fir_enroute { get; set; }
    }

    public class Fir_Etops
    {
    }

    public class Aircraft
    {
        public string icaocode { get; set; }
        public string iatacode { get; set; }
        public string name { get; set; }
        public string reg { get; set; }
        public string fin { get; set; }
        public Selcal selcal { get; set; }
        public string equip { get; set; }
        public string max_passengers { get; set; }
        public string fuelfact { get; set; }
    }

    public class Selcal
    {
    }

    public class Fuel
    {
        public string taxi { get; set; }
        public string enroute_burn { get; set; }
        public string contingency { get; set; }
        public string alternate_burn { get; set; }
        public string reserve { get; set; }
        public string etops { get; set; }
        public string extra { get; set; }
        public string min_takeoff { get; set; }
        public string plan_takeoff { get; set; }
        public string plan_ramp { get; set; }
        public string plan_landing { get; set; }
        public string avg_fuel_flow { get; set; }
        public string max_tanks { get; set; }
    }

    public class Times
    {
        public string est_time_enroute { get; set; }
        public string sched_time_enroute { get; set; }
        public string sched_out { get; set; }
        public string sched_off { get; set; }
        public string sched_on { get; set; }
        public string sched_in { get; set; }
        public string sched_block { get; set; }
        public string est_out { get; set; }
        public string est_off { get; set; }
        public string est_on { get; set; }
        public string est_in { get; set; }
        public string est_block { get; set; }
        public string orig_timezone { get; set; }
        public string dest_timezone { get; set; }
        public string taxi_out { get; set; }
        public string taxi_in { get; set; }
        public string reserve_time { get; set; }
        public string endurance { get; set; }
        public string contfuel_time { get; set; }
        public string etopsfuel_time { get; set; }
        public string extrafuel_time { get; set; }
    }

    public class Weights
    {
        public string oew { get; set; }
        public string pax_count { get; set; }
        public string pax_weight { get; set; }
        public string cargo { get; set; }
        public string payload { get; set; }
        public string est_zfw { get; set; }
        public string max_zfw { get; set; }
        public string est_tow { get; set; }
        public string max_tow { get; set; }
        public string max_tow_struct { get; set; }
        public string tow_limit_code { get; set; }
        public string est_ldw { get; set; }
        public string max_ldw { get; set; }
        public string est_ramp { get; set; }
    }

    public class Impacts
    {
        public Minus_6000Ft minus_6000ft { get; set; }
        public Minus_4000Ft minus_4000ft { get; set; }
        public Minus_2000Ft minus_2000ft { get; set; }
        public Plus_2000Ft plus_2000ft { get; set; }
        public Plus_4000Ft plus_4000ft { get; set; }
        public Plus_6000Ft plus_6000ft { get; set; }
        public Higher_Ci higher_ci { get; set; }
        public Lower_Ci lower_ci { get; set; }
        public Zfw_Plus_1000 zfw_plus_1000 { get; set; }
        public Zfw_Minus_1000 zfw_minus_1000 { get; set; }
    }

    public class Minus_6000Ft
    {
        public string time_enroute { get; set; }
        public string time_difference { get; set; }
        public string enroute_burn { get; set; }
        public string burn_difference { get; set; }
        public string ramp_fuel { get; set; }
        public string initial_fl { get; set; }
        public string initial_tas { get; set; }
        public string initial_mach { get; set; }
        public string cost_index { get; set; }
    }

    public class Minus_4000Ft
    {
        public string time_enroute { get; set; }
        public string time_difference { get; set; }
        public string enroute_burn { get; set; }
        public string burn_difference { get; set; }
        public string ramp_fuel { get; set; }
        public string initial_fl { get; set; }
        public string initial_tas { get; set; }
        public string initial_mach { get; set; }
        public string cost_index { get; set; }
    }

    public class Minus_2000Ft
    {
        public string time_enroute { get; set; }
        public string time_difference { get; set; }
        public string enroute_burn { get; set; }
        public string burn_difference { get; set; }
        public string ramp_fuel { get; set; }
        public string initial_fl { get; set; }
        public string initial_tas { get; set; }
        public string initial_mach { get; set; }
        public string cost_index { get; set; }
    }

    public class Plus_2000Ft
    {
        public string time_enroute { get; set; }
        public string time_difference { get; set; }
        public string enroute_burn { get; set; }
        public string burn_difference { get; set; }
        public string ramp_fuel { get; set; }
        public string initial_fl { get; set; }
        public string initial_tas { get; set; }
        public string initial_mach { get; set; }
        public string cost_index { get; set; }
    }

    public class Plus_4000Ft
    {
        public string time_enroute { get; set; }
        public string time_difference { get; set; }
        public string enroute_burn { get; set; }
        public string burn_difference { get; set; }
        public string ramp_fuel { get; set; }
        public string initial_fl { get; set; }
        public string initial_tas { get; set; }
        public string initial_mach { get; set; }
        public string cost_index { get; set; }
    }

    public class Plus_6000Ft
    {
        public string time_enroute { get; set; }
        public string time_difference { get; set; }
        public string enroute_burn { get; set; }
        public string burn_difference { get; set; }
        public string ramp_fuel { get; set; }
        public string initial_fl { get; set; }
        public string initial_tas { get; set; }
        public string initial_mach { get; set; }
        public string cost_index { get; set; }
    }

    public class Higher_Ci
    {
        public string time_enroute { get; set; }
        public string time_difference { get; set; }
        public string enroute_burn { get; set; }
        public string burn_difference { get; set; }
        public string ramp_fuel { get; set; }
        public string initial_fl { get; set; }
        public string initial_tas { get; set; }
        public string initial_mach { get; set; }
        public string cost_index { get; set; }
    }

    public class Lower_Ci
    {
        public string time_enroute { get; set; }
        public string time_difference { get; set; }
        public string enroute_burn { get; set; }
        public string burn_difference { get; set; }
        public string ramp_fuel { get; set; }
        public string initial_fl { get; set; }
        public string initial_tas { get; set; }
        public string initial_mach { get; set; }
        public string cost_index { get; set; }
    }

    public class Zfw_Plus_1000
    {
        public string time_enroute { get; set; }
        public string time_difference { get; set; }
        public string enroute_burn { get; set; }
        public string burn_difference { get; set; }
        public string ramp_fuel { get; set; }
        public string initial_fl { get; set; }
        public string initial_tas { get; set; }
        public string initial_mach { get; set; }
        public string cost_index { get; set; }
    }

    public class Zfw_Minus_1000
    {
        public string time_enroute { get; set; }
        public string time_difference { get; set; }
        public string enroute_burn { get; set; }
        public string burn_difference { get; set; }
        public string ramp_fuel { get; set; }
        public string initial_fl { get; set; }
        public string initial_tas { get; set; }
        public string initial_mach { get; set; }
        public string cost_index { get; set; }
    }

    public class Crew
    {
        public string pilot_id { get; set; }
        public string cpt { get; set; }
        public string fo { get; set; }
        public string dx { get; set; }
        public string pu { get; set; }
        public string[] fa { get; set; }
    }

    public class Notams
    {
        public Notamdrec[] notamdrec { get; set; }
        public string reccount { get; set; }
    }

    public class Notamdrec
    {
        public string source_id { get; set; }
        public string account_id { get; set; }
        public string notam_id { get; set; }
        public string notam_part { get; set; }
        public string cns_location_id { get; set; }
        public string icao_id { get; set; }
        public string icao_name { get; set; }
        public string total_parts { get; set; }
        public string notam_created_dtg { get; set; }
        public string notam_effective_dtg { get; set; }
        public string notam_expire_dtg { get; set; }
        public string notam_lastmod_dtg { get; set; }
        public string notam_inserted_dtg { get; set; }
        public string notam_text { get; set; }
        public string notam_report { get; set; }
        public string notam_nrc { get; set; }
        public string notam_qcode { get; set; }
    }

    public class Weather
    {
        public string orig_metar { get; set; }
        public string orig_taf { get; set; }
        public string dest_metar { get; set; }
        public string dest_taf { get; set; }
        public string altn_metar { get; set; }
        public string altn_taf { get; set; }
        public Toaltn_Metar toaltn_metar { get; set; }
        public Toaltn_Taf toaltn_taf { get; set; }
        public Eualtn_Metar eualtn_metar { get; set; }
        public Eualtn_Taf eualtn_taf { get; set; }
        public Etops_Metar etops_metar { get; set; }
        public Etops_Taf etops_taf { get; set; }
    }

    public class Toaltn_Metar
    {
    }

    public class Toaltn_Taf
    {
    }

    public class Eualtn_Metar
    {
    }

    public class Eualtn_Taf
    {
    }

    public class Etops_Metar
    {
    }

    public class Etops_Taf
    {
    }

    public class Sigmets
    {
    }

    public class Text
    {
        public Nat_Tracks nat_tracks { get; set; }
        public string plan_html { get; set; }
    }

    public class Nat_Tracks
    {
    }

    public class Tracks
    {
    }

    public class Database_Updates
    {
        public string metar_taf { get; set; }
        public string winds { get; set; }
        public string sigwx { get; set; }
        public string sigmet { get; set; }
        public string notams { get; set; }
        public string tracks { get; set; }
    }

    public class Files
    {
        public string directory { get; set; }
        public Pdf pdf { get; set; }
        public File[] file { get; set; }
    }

    public class Pdf
    {
        public string name { get; set; }
        public string link { get; set; }
    }

    public class File
    {
        public string name { get; set; }
        public string link { get; set; }
    }

    public class Fms_Downloads
    {
        public string directory { get; set; }
        public Pdf1 pdf { get; set; }
        public Abx abx { get; set; }
        public A3e a3e { get; set; }
        public Crx crx { get; set; }
        public Psx psx { get; set; }
        public Efb efb { get; set; }
        public Ef2 ef2 { get; set; }
        public Bbs bbs { get; set; }
        public Csf csf { get; set; }
        public Ftr ftr { get; set; }
        public Gtn gtn { get; set; }
        public Vm5 vm5 { get; set; }
        public Vmx vmx { get; set; }
        public Ffa ffa { get; set; }
        public Fsc fsc { get; set; }
        public Fs9 fs9 { get; set; }
        public Mfs mfs { get; set; }
        public Fsl fsl { get; set; }
        public Fsx fsx { get; set; }
        public Fsn fsn { get; set; }
        public Kml kml { get; set; }
        public Ify ify { get; set; }
        public I74 i74 { get; set; }
        public Ifa ifa { get; set; }
        public Ivo ivo { get; set; }
        public Xvd xvd { get; set; }
        public Xvp xvp { get; set; }
        public Ixg ixg { get; set; }
        public Jar jar { get; set; }
        public Mdr mdr { get; set; }
        public Mda mda { get; set; }
        public Lvd lvd { get; set; }
        public Mjc mjc { get; set; }
        public Mvz mvz { get; set; }
        public Vms vms { get; set; }
        public Pmr pmr { get; set; }
        public Pmw pmw { get; set; }
        public Mga mga { get; set; }
        public Psm psm { get; set; }
        public Qty qty { get; set; }
        public Sbr sbr { get; set; }
        public Sfp sfp { get; set; }
        public Tfd tfd { get; set; }
        public Ufc ufc { get; set; }
        public Vas vas { get; set; }
        public Vfp vfp { get; set; }
        public Wae wae { get; set; }
        public Xfm xfm { get; set; }
        public Xpe xpe { get; set; }
        public Xp9 xp9 { get; set; }
    }

    public class Pdf1
    {
        public string name { get; set; }
        public string link { get; set; }
    }

    public class Abx
    {
        public string name { get; set; }
        public string link { get; set; }
    }

    public class A3e
    {
        public string name { get; set; }
        public string link { get; set; }
    }

    public class Crx
    {
        public string name { get; set; }
        public string link { get; set; }
    }

    public class Psx
    {
        public string name { get; set; }
        public string link { get; set; }
    }

    public class Efb
    {
        public string name { get; set; }
        public string link { get; set; }
    }

    public class Ef2
    {
        public string name { get; set; }
        public string link { get; set; }
    }

    public class Bbs
    {
        public string name { get; set; }
        public string link { get; set; }
    }

    public class Csf
    {
        public string name { get; set; }
        public string link { get; set; }
    }

    public class Ftr
    {
        public string name { get; set; }
        public string link { get; set; }
    }

    public class Gtn
    {
        public string name { get; set; }
        public string link { get; set; }
    }

    public class Vm5
    {
        public string name { get; set; }
        public string link { get; set; }
    }

    public class Vmx
    {
        public string name { get; set; }
        public string link { get; set; }
    }

    public class Ffa
    {
        public string name { get; set; }
        public string link { get; set; }
    }

    public class Fsc
    {
        public string name { get; set; }
        public string link { get; set; }
    }

    public class Fs9
    {
        public string name { get; set; }
        public string link { get; set; }
    }

    public class Mfs
    {
        public string name { get; set; }
        public string link { get; set; }
    }

    public class Fsl
    {
        public string name { get; set; }
        public string link { get; set; }
    }

    public class Fsx
    {
        public string name { get; set; }
        public string link { get; set; }
    }

    public class Fsn
    {
        public string name { get; set; }
        public string link { get; set; }
    }

    public class Kml
    {
        public string name { get; set; }
        public string link { get; set; }
    }

    public class Ify
    {
        public string name { get; set; }
        public string link { get; set; }
    }

    public class I74
    {
        public string name { get; set; }
        public string link { get; set; }
    }

    public class Ifa
    {
        public string name { get; set; }
        public string link { get; set; }
    }

    public class Ivo
    {
        public string name { get; set; }
        public string link { get; set; }
    }

    public class Xvd
    {
        public string name { get; set; }
        public string link { get; set; }
    }

    public class Xvp
    {
        public string name { get; set; }
        public string link { get; set; }
    }

    public class Ixg
    {
        public string name { get; set; }
        public string link { get; set; }
    }

    public class Jar
    {
        public string name { get; set; }
        public string link { get; set; }
    }

    public class Mdr
    {
        public string name { get; set; }
        public string link { get; set; }
    }

    public class Mda
    {
        public string name { get; set; }
        public string link { get; set; }
    }

    public class Lvd
    {
        public string name { get; set; }
        public string link { get; set; }
    }

    public class Mjc
    {
        public string name { get; set; }
        public string link { get; set; }
    }

    public class Mvz
    {
        public string name { get; set; }
        public string link { get; set; }
    }

    public class Vms
    {
        public string name { get; set; }
        public string link { get; set; }
    }

    public class Pmr
    {
        public string name { get; set; }
        public string link { get; set; }
    }

    public class Pmw
    {
        public string name { get; set; }
        public string link { get; set; }
    }

    public class Mga
    {
        public string name { get; set; }
        public string link { get; set; }
    }

    public class Psm
    {
        public string name { get; set; }
        public string link { get; set; }
    }

    public class Qty
    {
        public string name { get; set; }
        public string link { get; set; }
    }

    public class Sbr
    {
        public string name { get; set; }
        public string link { get; set; }
    }

    public class Sfp
    {
        public string name { get; set; }
        public string link { get; set; }
    }

    public class Tfd
    {
        public string name { get; set; }
        public string link { get; set; }
    }

    public class Ufc
    {
        public string name { get; set; }
        public string link { get; set; }
    }

    public class Vas
    {
        public string name { get; set; }
        public string link { get; set; }
    }

    public class Vfp
    {
        public string name { get; set; }
        public string link { get; set; }
    }

    public class Wae
    {
        public string name { get; set; }
        public string link { get; set; }
    }

    public class Xfm
    {
        public string name { get; set; }
        public string link { get; set; }
    }

    public class Xpe
    {
        public string name { get; set; }
        public string link { get; set; }
    }

    public class Xp9
    {
        public string name { get; set; }
        public string link { get; set; }
    }

    public class Images
    {
        public string directory { get; set; }
    }

    public class Links
    {
        public string skyvector { get; set; }
    }

    public class Api_Params
    {
        public string airline { get; set; }
        public string fltnum { get; set; }
        public string type { get; set; }
        public string orig { get; set; }
        public string dest { get; set; }
        public string date { get; set; }
        public string dephour { get; set; }
        public string depmin { get; set; }
        public string route { get; set; }
        public string stehour { get; set; }
        public string stemin { get; set; }
        public string reg { get; set; }
        public string fin { get; set; }
        public Selcal1 selcal { get; set; }
        public string pax { get; set; }
        public string altn { get; set; }
        public string fl { get; set; }
        public string cpt { get; set; }
        public string pid { get; set; }
        public string fuelfactor { get; set; }
        public string manualzfw { get; set; }
        public string addedfuel { get; set; }
        public string contpct { get; set; }
        public string resvrule { get; set; }
        public string taxiout { get; set; }
        public string taxiin { get; set; }
        public string cargo { get; set; }
        public string origrwy { get; set; }
        public string destrwy { get; set; }
        public string climb { get; set; }
        public string descent { get; set; }
        public string cruisemode { get; set; }
        public string cruisesub { get; set; }
        public string planformat { get; set; }
        public string pounds { get; set; }
        public string navlog { get; set; }
        public string etops { get; set; }
        public string stepclimbs { get; set; }
        public Tlr tlr { get; set; }
        public string notams_opt { get; set; }
        public string firnot { get; set; }
        public string maps { get; set; }
        public Turntoflt turntoflt { get; set; }
        public Turntoapt turntoapt { get; set; }
        public Turntotime turntotime { get; set; }
        public Turnfrflt turnfrflt { get; set; }
        public Turnfrapt turnfrapt { get; set; }
        public Turnfrtime turnfrtime { get; set; }
        public Fuelstats fuelstats { get; set; }
        public Contlabel contlabel { get; set; }
        public Static_Id2 static_id { get; set; }
        public Acdata acdata { get; set; }
        public Acdata_Parsed acdata_parsed { get; set; }
    }

    public class Selcal1
    {
    }

    public class Tlr
    {
    }

    public class Turntoflt
    {
    }

    public class Turntoapt
    {
    }

    public class Turntotime
    {
    }

    public class Turnfrflt
    {
    }

    public class Turnfrapt
    {
    }

    public class Turnfrtime
    {
    }

    public class Fuelstats
    {
    }

    public class Contlabel
    {
    }

    public class Static_Id2
    {
    }

    public class Acdata
    {
    }

    public class Acdata_Parsed
    {
    }

    public class Metar
    {
        public string sanitized { get; set; }
    }


}
