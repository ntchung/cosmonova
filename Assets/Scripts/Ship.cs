using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ship : MonoBehaviour 
{
	int Game_skill_level = NUM_SKILL_LEVELS - 1;
	int Missiontime = 0;

	const int NUM_SKILL_LEVELS = 5;

	const float MAX_TURN_LIMIT = 0.2618f;
	
	const float ROTVEL_TOL = 0.1f;
	const float ROTVEL_CAP = 14.0f;
	const float DEAD_ROTVEL_CAP = 16.3f;
	
	const float MAX_SHIP_SPEED = 500.0f;
	const float RESET_SHIP_SPEED = 440.0f;
	
	const int SW_ROT_FACTOR = 5;
	const int SW_BLAST_DURATION	= 2000;
	const int REDUCED_DAMP_FACTOR = 10;
	const int REDUCED_DAMP_VEL = 30;
	const int REDUCED_DAMP_TIME	= 2000;
	const int WEAPON_SHAKE_TIME = 500;
	const float SPECIAL_WARP_T_CONST = 0.651f;

	const int PF_ACCELERATES = (1 << 1);
	const int PF_USE_VEL = (1 << 2);
	const int PF_AFTERBURNER_ON	= (1 << 3);
	const int PF_SLIDE_ENABLED = (1 << 4);
	const int PF_REDUCED_DAMP = (1 << 5);	
	const int PF_IN_SHOCKWAVE = (1 << 6);		
	const int PF_DEAD_DAMP = (1 << 7);	
	const int PF_AFTERBURNER_WAIT = (1 << 8);
	const int PF_CONST_VEL = (1 << 9);
	const int PF_WARP_IN = (1 << 10);	
	const int PF_SPECIAL_WARP_IN = (1 << 11);
	const int PF_WARP_OUT = (1 << 12);
	const int PF_SPECIAL_WARP_OUT = (1 << 13);
	const int PF_BOOSTER_ON	= (1 << 14);
	const int PF_GLIDING = (1 << 15);
	const int PF_FORCE_GLIDE = (1 << 16);

	const int AIPF_SMART_SHIELD_MANAGEMENT = (1 << 0);
	const int AIPF_BIG_SHIPS_CAN_ATTACK_BEAM_TURRETS_ON_UNTARGETED_SHIPS = (1 << 1);
	const int AIPF_SMART_PRIMARY_WEAPON_SELECTION = (1 << 2);
	const int AIPF_SMART_SECONDARY_WEAPON_SELECTION	= (1 << 3);
	const int AIPF_ALLOW_RAPID_SECONDARY_DUMBFIRE = (1 << 4);
	const int AIPF_HUGE_TURRET_WEAPONS_IGNORE_BOMBS	= (1 << 5);
	const int AIPF_DONT_INSERT_RANDOM_TURRET_FIRE_DELAY = (1 << 6);
	const int AIPF_HACK_IMPROVE_NON_HOMING_SWARM_TURRET_FIRE_ACCURACY = (1 << 7);
	const int AIPF_SHOCKWAVES_DAMAGE_SMALL_SHIP_SUBSYSTEMS = (1 << 8);
	const int AIPF_NAVIGATION_SUBSYS_GOVERNS_WARP = (1 << 9);
	const int AIPF_NO_MIN_DOCK_SPEED_CAP = (1 << 10);
	const int AIPF_DISABLE_LINKED_FIRE_PENALTY = (1 << 11);
	const int AIPF_DISABLE_WEAPON_DAMAGE_SCALING = (1 << 12);
	const int AIPF_USE_ADDITIVE_WEAPON_VELOCITY	= (1 << 13);
	const int AIPF_USE_NEWTONIAN_DAMPENING = (1 << 14);
	const int AIPF_INCLUDE_BEAMS_IN_STAT_CALCS = (1 << 15);
	const int AIPF_KILL_SCORING_SCALES_WITH_DAMAGE = (1 << 16);
	const int AIPF_ASSIST_SCORING_SCALES_WITH_DAMAGE = (1 << 17);
	const int AIPF_ALLOW_MULTI_EVENT_SCORING = (1 << 18);
	const int AIPF_SMART_AFTERBURNER_MANAGEMENT = (1 << 19);
	const int AIPF_FIX_LINKED_PRIMARY_BUG = (1 << 20);
	const int AIPF_PREVENT_TARGETING_BOMBS_BEYOND_RANGE = (1 << 21);
	const int AIPF_SMART_SUBSYSTEM_TARGETING_FOR_TURRETS = (1 << 22);
	const int AIPF_FIX_HEAT_SEEKER_STEALTH_BUG = (1 << 23);
	const int AIPF_MULTI_ALLOW_EMPTY_PRIMARIES = (1 << 24);
	const int AIPF_MULTI_ALLOW_EMPTY_SECONDARIES = (1 << 25);
	const int AIPF_ALLOW_TURRETS_TARGET_WEAPONS_FREELY = (1 << 26);
	const int AIPF_USE_ONLY_SINGLE_FOV_FOR_TURRETS = (1 << 27);
	const int AIPF_ALLOW_VERTICAL_DODGE = (1 << 28);
	const int AIPF_FORCE_BEAM_TURRET_FOV = (1 << 29);
	const int AIPF_FIX_AI_CLASS_BUG	= (1 << 30);

	const int AIPF2_TURRETS_IGNORE_TARGET_RADIUS = (1 << 0);
	const int AIPF2_NO_SPECIAL_PLAYER_AVOID = (1 << 1);
	const int AIPF2_PERFORM_FEWER_SCREAM_CHECKS = (1 << 2);
	const int AIPF2_ALL_SHIPS_MANAGE_SHIELDS = (1 << 3);
	const int AIPF2_ADVANCED_TURRET_FOV_EDGE_CHECKS = (1 << 4);
	const int AIPF2_REQUIRE_TURRET_TO_HAVE_TARGET_IN_FOV = (1 << 5);
	const int AIPF2_AI_AIMS_FROM_SHIP_CENTER = (1 << 6);
	const int AIPF2_ALLOW_PRIMARY_LINK_AT_START = (1 << 7);
	const int AIPF2_BEAMS_DAMAGE_WEAPONS = (1 << 8);
	const int AIPF2_PLAYER_WEAPON_SCALE_FIX	= (1 << 9);
	const int AIPF2_NO_WARP_CAMERA = (1 << 10);
	const int AIPF2_ASPECT_LOCK_COUNTERMEASURE = (1 << 11);
	const int AIPF2_AI_GUARDS_SPECIFIC_SHIP_IN_WING = (1 << 12);
	const int AIPF2_FIX_AI_PATH_ORDER_BUG = (1 << 13);

	const int OBJ_NONE = 0;
	const int OBJ_SHIP = 1;
	const int OBJ_WEAPON = 2;
	const int OBJ_FIREBALL = 3;
	const int OBJ_START = 4;
	const int OBJ_WAYPOINT = 5;
	const int OBJ_DEBRIS = 6;
	const int OBJ_CMEASURE = 7;
	const int OBJ_GHOST = 8;
	const int OBJ_POINT = 9;
	const int OBJ_SHOCKWAVE = 10;
	const int OBJ_WING = 11;
	const int OBJ_OBSERVER = 12;
	const int OBJ_ASTEROID = 13;
	const int OBJ_JUMP_NODE = 14;
	const int OBJ_BEAM = 15;

	const int AIM_CHASE = 0;
	const int AIM_EVADE = 1;
	const int AIM_GET_BEHIND = 2;
	const int AIM_STAY_NEAR = 3;
	const int AIM_STILL = 4;
	const int AIM_GUARD = 5;
	const int AIM_AVOID = 6;
	const int AIM_WAYPOINTS = 7;
	const int AIM_DOCK = 8;
	const int AIM_NONE = 9;
	const int AIM_BIGSHIP = 10;
	const int AIM_PATH = 11;
	const int AIM_BE_REARMED = 12;
	const int AIM_SAFETY = 13;
	const int AIM_EVADE_WEAPON = 14;
	const int AIM_STRAFE = 15;
	const int AIM_PLAY_DEAD = 16;
	const int AIM_BAY_EMERGE = 17;
	const int AIM_BAY_DEPART = 18;
	const int AIM_SENTRYGUN = 19;
	const int AIM_WARP_OUT = 20;
	const int AIM_FLY_TO_SHIP = 21;
	const int MAX_AI_BEHAVIORS = 22;

	const int OF_RENDERS = (1<<0);	
	const int OF_COLLIDES = (1<<1);
	const int OF_PHYSICS = (1<<2);
	const int OF_SHOULD_BE_DEAD = (1<<3);
	const int OF_INVULNERABLE = (1<<4);
	const int OF_PROTECTED = (1<<5);
	const int OF_PLAYER_SHIP = (1<<6);
	const int OF_NO_SHIELDS = (1<<7);	
	const int OF_JUST_UPDATED = (1<<8);
	const int OF_COULD_BE_PLAYER = (1<<9);
	const int OF_WAS_RENDERED = (1<<10);
	const int OF_NOT_IN_COLL = (1<<11);
	const int OF_BEAM_PROTECTED	= (1<<12);
	const int OF_SPECIAL_WARPIN	= (1<<13);	
	const int OF_DOCKED_ALREADY_HANDLED = (1<<14);
	const int OF_TARGETABLE_AS_BOMB = (1<<15);
	const int OF_FLAK_PROTECTED	= (1<<16);
	const int OF_LASER_PROTECTED = (1<<17);
	const int OF_MISSILE_PROTECTED = (1<<18);
	const int OF_IMMOBILE = (1<<19);	

#region Physics
	int p_flags;
	
	float mass;
	Vector3 center_of_mass;
	Matrix I_body_inv;	
	
	float rotdamp;
	float side_slip_time_const;

	float delta_bank_const;
	
	Vector3	max_vel;
	Vector3 afterburner_max_vel;
	Vector3 booster_max_vel;
	Vector3	max_rotvel;
	float max_rear_vel;
	
	float forward_accel_time_const;
	float afterburner_forward_accel_time_const;
	float booster_forward_accel_time_const;
	float forward_decel_time_const;
	float slide_accel_time_const;
	float slide_decel_time_const;
	float shockwave_shake_amp;	
	
	Vector3	prev_ramp_vel;			
	Vector3 desired_vel;			
	Vector3	desired_rotvel;	
	float forward_thrust;
	float side_thrust;
	float vert_thrust;
	
	Vector3 vel;
	Vector3	rotvel;	
	float speed;		
	float fspeed;
	float heading;
	Vector3 prev_fvec;	
	Matrix last_rotmat;	
	
	int	afterburner_decay;
	int	shockwave_decay;
	int	reduced_damp_decay;
	
	float glide_cap;
	float cur_glide_cap;
	float glide_accel_mult;
	bool use_newtonian_damp;
	float afterburner_max_reverse_vel;
	float afterburner_reverse_accel;
#endregion

#region AI
	int	ai_flags;	

	int	behavior;				
	int	mode;
	int	previous_mode;
	int	mode_time;		
	object target_obj;
	object previous_target_obj;

	float previous_dot_to_enemy;
	float target_time;

	object attacker_obj;
	
	object guard_obj;	

	int submode;
	int	previous_submode;		
	float best_dot_to_enemy;
	float best_dot_from_enemy;
	int	best_dot_to_time;	
	int	best_dot_from_time;	
	int	submode_start_time;	
	int	submode_parm0;		
	int	submode_parm1;		
	int	next_predict_pos_time;
	
	int	next_aim_pos_time;
	Vector3 last_aim_enemy_pos;
	Vector3 last_aim_enemy_vel;

	Vector3	last_predicted_enemy_pos;
	float time_enemy_in_range;
	float time_enemy_near;	
	int	last_attack_time;	
	int	last_hit_time;		
	int	last_hit_quadrant;	
	int	last_hit_target_time;

	object hitter_obj;

	float prev_accel;

	float ai_accuracy, ai_evasion, ai_courage, ai_patience;
	int	ai_aburn_use_factor;		
	float ai_shockwave_evade_chance;	
	float ai_get_away_chance;	
	float ai_secondary_range_mult;
	bool ai_class_autoscale;

	float ai_cmeasure_fire_chance;
	float ai_in_range_time;
	float ai_link_ammo_levels_maybe;
	float ai_link_ammo_levels_always;
	float ai_primary_ammo_burst_mult;
	float ai_link_energy_levels_maybe;
	float ai_link_energy_levels_always;
	int	ai_predict_position_delay;
	float ai_shield_manage_delay;	
	float ai_ship_fire_delay_scale_friendly;
	float ai_ship_fire_delay_scale_hostile;
	float ai_ship_fire_secondary_delay_scale_friendly;
	float ai_ship_fire_secondary_delay_scale_hostile;
	float ai_turn_time_scale;
	float ai_glide_attack_percent;
	float ai_circle_strafe_percent;
	float ai_glide_strafe_percent;
	float ai_random_sidethrust_percent;
	float ai_stalemate_time_thresh;
	float ai_stalemate_dist_thresh;
	int	ai_chance_to_use_missiles_on_plr;
	float ai_max_aim_update_delay;
	float ai_turret_max_aim_update_delay;
	int	ai_profile_flags;
	int	ai_profile_flags2;	

	float aspect_locked_time;	

	Vector3	guard_vec;	
	int nearest_locked_object;		
	float nearest_locked_distance;	
	
	float current_target_distance;
	int	current_target_is_locked;
	int	current_target_dist_trend;
	int	current_target_speed_trend;
	
	float last_dist;				
	float last_speed;				
	int	last_secondary_index;		
	int	last_target;
	
	bool rearm_first_missile;				
	bool rearm_first_ballistic_primary;		
	int	rearm_release_delay;				
	
	int	afterburner_stop_time;			
	int	ignore_expire_timestamp;	
	int	warp_out_timestamp;			
	int	next_rearm_request_timestamp;
	int	primary_select_timestamp;	
	int	secondary_select_timestamp;	
	
	int	scan_for_enemy_timestamp;
	int	choose_enemy_timestamp;

	int	force_warp_time;					
	
	int	shield_manage_timestamp;
	int	self_destruct_timestamp;
	int	ok_to_target_timestamp;	
	
	int	kamikaze_damage;

	int avoid_check_timestamp;

	Vector3	big_collision_normal;		
	Vector3	big_recover_pos_1;
	Vector3	big_recover_pos_2;
	int big_recover_timestamp;		
	
	int	abort_rearm_timestamp;		
	
	object artillery_obj;
	float artillery_lock_time;
	Vector3 artillery_lock_pos;
	float lethality;		
#endregion

	object En_obj;

	int team;
	int type;	

	int flags;	
	Vector3	pos;		
	Matrix orient;	
	float radius;
	Vector3 last_pos;
	Matrix last_orient;

	public void physics_init()
	{
		mass = 10.0f;					
		side_slip_time_const = 0.05f;
		rotdamp = 0.1f;
		
		max_vel.x = 100.0f;		
		max_vel.y = 100.0f;	
		max_vel.z = 100.0f;	
		max_rear_vel = 100.0f;
		
		max_rotvel.x = 2.0f;	
		max_rotvel.y = 1.0f;	
		max_rotvel.z = 2.0f;
		
		prev_ramp_vel.x = 0.0f;
		prev_ramp_vel.y = 0.0f;
		prev_ramp_vel.z = 0.0f;
		
		desired_vel.x = 0.0f;
		desired_vel.y = 0.0f;
		desired_vel.z = 0.0f;
		
		slide_accel_time_const = side_slip_time_const;
		slide_decel_time_const = side_slip_time_const;
		
		afterburner_decay = 1;
		forward_thrust = 0.0f;
		vert_thrust = 0.0f;	
		side_thrust = 0.0f;	
		
		p_flags = 0;

		I_body_inv.rvec = new Vector3(1e-5f, 0.0f, 0.0f);
		I_body_inv.uvec = new Vector3(0.0f, 1e-5f, 0.0f);
		I_body_inv.fvec = new Vector3(0.0f, 0.0f, 1e-5f);

		float model_mass = 117.7239f;
		Vector3 model_center_of_mass = Vector3.zero;
		Matrix model_moment_of_inertia = new Matrix();
		model_moment_of_inertia.SetZero();

		float ship_density  = 1.0f;
		float ship_damp = 0.4f;
		float ship_rotdamp = 0.65f;

		float ship_delta_bank_const = 0.5f;

		Vector3 ship_max_vel = new Vector3(22.5f, 22.5f, 90.0f);

		Vector3 ship_rotation_time = new Vector3(3.8f, 3.8f, 3.1f);
		float ship_srotation_time = (ship_rotation_time.x + ship_rotation_time.y) / 2.0f;
		Vector3 ship_max_rotvel = new Vector3(
			(2 * Mathf.PI) / ship_rotation_time.x,
			(2 * Mathf.PI) / ship_rotation_time.y,
			(2 * Mathf.PI) / ship_rotation_time.z);
		
		float ship_max_rear_vel = 45.0f;
		float ship_min_speed = -ship_max_rear_vel;

		float ship_forward_accel = 3.0f;
		float ship_forward_decel = 2.5f;
		float ship_slide_accel = 1.2f;
		float ship_slide_decel = 0.8f;

		bool ship_can_glide = true;
		float ship_glide_cap = 0.0f;
		bool ship_glide_dynamic_cap = false;
		float ship_glide_accel_mult = 0.0f;
		bool ship_use_newtonian_damp = false;
		bool ship_newtonian_damp_override = false;

		float ship_max_overclocked_speed = ship_max_vel.z * 1.5f;

		Vector3 ship_afterburner_max_vel = new Vector3(0.0f, 0.0f, 125.0f);
		float ship_afterburner_forward_accel = 1.0f;
		float ship_afterburner_fuel_capacity = 250.0f;
		float ship_afterburner_burn_rate = 75.0f;
		float ship_afterburner_recover_rate = 25.0f;
		float ship_afterburner_max_reverse_vel = 0.0f;
		float ship_afterburner_reverse_accel = 0.0f;

		int ship_cmeasure_max = 6;
		int ship_scan_time = 2000;

		mass = model_mass * ship_density;

		I_body_inv = model_moment_of_inertia;
		I_body_inv.rvec *= ship_density;
		I_body_inv.uvec *= ship_density;
		I_body_inv.fvec *= ship_density;

		center_of_mass = model_center_of_mass;
		side_slip_time_const = ship_damp;
		delta_bank_const = ship_delta_bank_const;
		rotdamp = ship_rotdamp;
		max_vel = ship_max_vel;
		afterburner_max_vel = ship_afterburner_max_vel;
		max_rotvel = ship_max_rotvel;
		max_rear_vel = ship_max_rear_vel;

		p_flags |= PF_ACCELERATES;	
		p_flags &= ~PF_GLIDING;
		p_flags &= ~PF_FORCE_GLIDE;
		
		forward_accel_time_const = ship_forward_accel;
		afterburner_forward_accel_time_const = ship_afterburner_forward_accel;
		forward_decel_time_const = ship_forward_decel;
		slide_accel_time_const = ship_slide_accel;
		slide_decel_time_const = ship_slide_decel;

		if (ship_max_vel.x > 0.000001f || ship_max_vel.y > 0.000001f) p_flags |= PF_SLIDE_ENABLED;
		
		cur_glide_cap = max_vel.z; 
		if (ship_glide_cap > 0.000001f || ship_glide_cap < -0.000001f) glide_cap = ship_glide_cap;
		else glide_cap = Mathf.Max(Mathf.Max(max_vel.z, ship_max_overclocked_speed), afterburner_max_vel.z);

		if (ship_glide_dynamic_cap)	glide_cap = 0.0f;
		
		glide_accel_mult = ship_glide_accel_mult;
		
		use_newtonian_damp = false;

		vel = Vector3.zero;
		rotvel = Vector3.zero;
		speed = 0.0f;
		heading = 0.0f;

		last_rotmat.SetIdentity();

		afterburner_max_reverse_vel = ship_afterburner_max_reverse_vel;
		afterburner_reverse_accel = ship_afterburner_reverse_accel;
	}

	readonly float[] accuracy = {0.85f, 0.9f, 0.95f, 0.96f, 0.99f};
	public readonly float[] evasion = {37, 47, 57, 67, 77};
	public readonly float[] courage = {10, 15, 20, 25, 30};
	public readonly float[] patience = {25, 35, 45, 55, 65};

	public readonly int[] aburn_use_factor = {int.MinValue, int.MinValue, int.MinValue, int.MinValue, int.MinValue};
	public readonly float[] shockwave_evade_chance = {float.MinValue, float.MinValue, float.MinValue, float.MinValue, float.MinValue};
	public readonly float[] get_away_chance = {float.MinValue, float.MinValue, float.MinValue, float.MinValue, float.MinValue};
	public readonly float[] secondary_range_mult = {float.MinValue, float.MinValue, float.MinValue, float.MinValue, float.MinValue};

	/*
	public readonly float[] cmeasure_fire_chance = {float.MinValue, float.MinValue, float.MinValue, float.MinValue, float.MinValue};
	public readonly float[] in_range_time = {float.MinValue, float.MinValue, float.MinValue, float.MinValue, float.MinValue};
	public readonly float[] link_ammo_levels_maybe = {float.MinValue, float.MinValue, float.MinValue, float.MinValue, float.MinValue};
	public readonly float[] link_ammo_levels_always = {float.MinValue, float.MinValue, float.MinValue, float.MinValue, float.MinValue};
	*/
	public readonly float[] primary_ammo_burst_mult = {float.MinValue, float.MinValue, float.MinValue, float.MinValue, float.MinValue};
	/*
	public readonly float[] link_energy_levels_maybe = {float.MinValue, float.MinValue, float.MinValue, float.MinValue, float.MinValue};
	public readonly float[] link_energy_levels_always = {float.MinValue, float.MinValue, float.MinValue, float.MinValue, float.MinValue};
	public readonly int[] predict_position_delay = {int.MinValue, int.MinValue, int.MinValue, int.MinValue, int.MinValue};
	public readonly float[] shield_manage_delay = {float.MinValue, float.MinValue, float.MinValue, float.MinValue, float.MinValue};
	public readonly float[] ship_fire_delay_scale_friendly = {float.MinValue, float.MinValue, float.MinValue, float.MinValue, float.MinValue};
	public readonly float[] ship_fire_delay_scale_hostile = {float.MinValue, float.MinValue, float.MinValue, float.MinValue, float.MinValue};
	*/
	public readonly float[] ship_fire_secondary_delay_scale_friendly = {float.MinValue, float.MinValue, float.MinValue, float.MinValue, float.MinValue};
	public readonly float[] ship_fire_secondary_delay_scale_hostile = {float.MinValue, float.MinValue, float.MinValue, float.MinValue, float.MinValue};
	/*
	public readonly float[] turn_time_scale = {float.MinValue, float.MinValue, float.MinValue, float.MinValue, float.MinValue};
	*/

	public readonly float[] glide_attack_percent = {float.MinValue, float.MinValue, float.MinValue, float.MinValue, float.MinValue};
	public readonly float[] circle_strafe_percent = {float.MinValue, float.MinValue, float.MinValue, float.MinValue, float.MinValue};
	public readonly float[] glide_strafe_percent = {float.MinValue, float.MinValue, float.MinValue, float.MinValue, float.MinValue};
	public readonly float[] random_sidethrust_percent = {float.MinValue, float.MinValue, float.MinValue, float.MinValue, float.MinValue};
	public readonly float[] stalemate_time_thresh = {float.MinValue, float.MinValue, float.MinValue, float.MinValue, float.MinValue};
	public readonly float[] stalemate_dist_thresh = {float.MinValue, float.MinValue, float.MinValue, float.MinValue, float.MinValue};
	public readonly int[] chance_to_use_missiles_on_plr = {int.MinValue, int.MinValue, int.MinValue, int.MinValue, int.MinValue};
	public readonly float[] max_aim_update_delay = {float.MinValue, float.MinValue, float.MinValue, float.MinValue, float.MinValue};
	public readonly float[] turret_max_aim_update_delay = {float.MinValue, float.MinValue, float.MinValue, float.MinValue, float.MinValue};

	public const bool class_autoscale = true;

	public readonly float[] afterburner_recharge_scale = {5.0f, 3.0f, 2.0f, 1.5f, 1.0f};
	public readonly float[] beam_friendly_damage_cap = {0, 5, 10, 20, 30};
	public readonly float[] cmeasure_life_scale = {3.0f, 2.0f, 1.5f, 1.25f, 1.0f};
	public readonly float[] cmeasure_fire_chance = {0.2f, 0.3f, 0.5f, 0.9f, 1.1f};
	public readonly float[] in_range_time = {2.0f, 1.4f, 0.75f, 0.0f, -1.0f};
	public readonly float[] link_ammo_levels_always = {95, 80, 60, 40, 20};
	public readonly float[] link_ammo_levels_maybe = {90, 60, 40, 20, 10};
	public readonly float[] link_energy_levels_always = {100, 80, 60, 40, 20};
	public readonly float[] link_energy_levels_maybe = {90, 60, 40, 20, 10};
	public readonly int[] max_allowed_player_homers = {2, 3, 4, 7, 99};
	public readonly int[] max_attackers = {2, 3, 4, 5, 99};
	public readonly int[] max_incoming_asteroids = {1, 1, 2, 2, 3};
	public readonly float[] player_damage_scale = {0.25f, 0.5f, 0.65f, 0.85f, 1.0f};
	public readonly float[] subsys_damage_scale = {0.2f, 0.4f, 0.6f, 0.8f, 1.0f};
	public readonly int[] predict_position_delay = {2 * 65536, (int)(1.5f * 65536), (int)(1.33f * 65536), (int)(0.5f * 65536), 0};
	public readonly float[] shield_manage_delay = {5.0f, 4.0f, 2.5f, 1.2f, 0.1f};
	public readonly float[] ship_fire_delay_scale_friendly = {2.0f, 1.4f, 1.25f, 1.1f, 1.0f};
	public readonly float[] ship_fire_delay_scale_hostile = {4.0f, 2.5f, 1.75f, 1.25f, 1.0f};
	public readonly float[] turn_time_scale = {3, 2.2f, 1.6f, 1.3f, 1.0f};

	public int profile_flags = 
		AIPF_BIG_SHIPS_CAN_ATTACK_BEAM_TURRETS_ON_UNTARGETED_SHIPS |
		AIPF_SMART_PRIMARY_WEAPON_SELECTION |
		AIPF_SMART_SECONDARY_WEAPON_SELECTION |
		AIPF_DONT_INSERT_RANDOM_TURRET_FIRE_DELAY |
		AIPF_HACK_IMPROVE_NON_HOMING_SWARM_TURRET_FIRE_ACCURACY |
		AIPF_SHOCKWAVES_DAMAGE_SMALL_SHIP_SUBSYSTEMS;

	public int profile_flags2 = 0;

	public void ai_init()
	{
		Vector3 near_vec = pos + orient.fvec * 100.0f + orient.rvec * 10.0f;

		ai_flags = 0;
		previous_mode = AIM_NONE;
		mode_time = -1;
		target_obj = null;
		previous_target_obj = null;

		target_time = 0.0f;

		attacker_obj = null;
		guard_obj = null;

		submode = 0;
		previous_submode = 0;
		best_dot_to_enemy = -1.0f;
		best_dot_from_enemy = -1.0f;
		best_dot_to_time = 0;
		best_dot_from_time = 0;
		submode_start_time = 0;
		submode_parm0 = 0;
		submode_parm1 = 0;

		last_predicted_enemy_pos = near_vec;

		time_enemy_in_range = 0.0f;
		time_enemy_near = 0.0f;
		last_attack_time = 0;
		last_hit_time = 0;
		last_hit_quadrant = 0;

		hitter_obj = null;

		prev_accel = 0.0f;

		ai_courage = courage[Game_skill_level];
		ai_patience = patience[Game_skill_level];
		ai_evasion = evasion[Game_skill_level];
		ai_accuracy = accuracy[Game_skill_level];
		
		ai_aburn_use_factor = aburn_use_factor[Game_skill_level];		
		ai_shockwave_evade_chance = shockwave_evade_chance[Game_skill_level];	
		ai_get_away_chance = get_away_chance[Game_skill_level];	
		ai_secondary_range_mult = secondary_range_mult[Game_skill_level];

		ai_class_autoscale = class_autoscale;

		ai_cmeasure_fire_chance = cmeasure_fire_chance[Game_skill_level];
		ai_in_range_time = in_range_time[Game_skill_level];
		ai_link_ammo_levels_maybe = link_ammo_levels_maybe[Game_skill_level];
		ai_link_ammo_levels_always = link_ammo_levels_always[Game_skill_level];
		ai_primary_ammo_burst_mult = primary_ammo_burst_mult[Game_skill_level];
		ai_link_energy_levels_maybe = link_energy_levels_maybe[Game_skill_level];
		ai_link_energy_levels_always = link_energy_levels_always[Game_skill_level];
		ai_predict_position_delay = predict_position_delay[Game_skill_level];
		ai_shield_manage_delay = shield_manage_delay[Game_skill_level];
		ai_ship_fire_delay_scale_friendly = ship_fire_delay_scale_friendly[Game_skill_level];
		ai_ship_fire_delay_scale_hostile = ship_fire_delay_scale_hostile[Game_skill_level];
		ai_ship_fire_secondary_delay_scale_friendly = ship_fire_secondary_delay_scale_friendly[Game_skill_level];
		ai_ship_fire_secondary_delay_scale_hostile = ship_fire_secondary_delay_scale_hostile[Game_skill_level];
		ai_turn_time_scale = turn_time_scale[Game_skill_level];
		ai_glide_attack_percent = glide_attack_percent[Game_skill_level];
		ai_circle_strafe_percent = circle_strafe_percent[Game_skill_level];
		ai_glide_strafe_percent = glide_strafe_percent[Game_skill_level];
		ai_random_sidethrust_percent = random_sidethrust_percent[Game_skill_level];
		ai_stalemate_time_thresh = stalemate_time_thresh[Game_skill_level];
		ai_stalemate_dist_thresh = stalemate_dist_thresh[Game_skill_level];
		ai_chance_to_use_missiles_on_plr = chance_to_use_missiles_on_plr[Game_skill_level];
		ai_max_aim_update_delay = max_aim_update_delay[Game_skill_level];
		ai_turret_max_aim_update_delay = turret_max_aim_update_delay[Game_skill_level];
		
		ai_profile_flags = profile_flags;
		ai_profile_flags2 = profile_flags2;

		prev_fvec = orient.fvec;
		
		last_hit_target_time = Missiontime;
		last_hit_time = Missiontime;
		
		nearest_locked_object = -1;
		nearest_locked_distance = 99999.0f;

		rearm_first_missile = true;
		rearm_first_ballistic_primary = true;	
		rearm_release_delay = 0;
		
		next_predict_pos_time = 0;
		next_aim_pos_time = 0;
		
		afterburner_stop_time = 0;

		ignore_expire_timestamp = (int)(Time.realtimeSinceStartup * 1000.0f);
		next_rearm_request_timestamp = (int)(Time.realtimeSinceStartup * 1000.0f);
		primary_select_timestamp = (int)(Time.realtimeSinceStartup * 1000.0f);
		secondary_select_timestamp = (int)(Time.realtimeSinceStartup * 1000.0f);
		scan_for_enemy_timestamp = (int)(Time.realtimeSinceStartup * 1000.0f);
		
		choose_enemy_timestamp = 
			(int)(Time.realtimeSinceStartup * 100.0f) + 
			(3 * (NUM_SKILL_LEVELS - Game_skill_level) * (UnityEngine.Random.Range(0, 500) + 500));
	
		abort_rearm_timestamp = -1;

		shield_manage_timestamp = (int)(Time.realtimeSinceStartup * 1000.0f);
		self_destruct_timestamp = -1;	
		ok_to_target_timestamp = (int)(Time.realtimeSinceStartup * 1000.0f);

		avoid_check_timestamp = (int)(Time.realtimeSinceStartup * 1000.0f);
		
		abort_rearm_timestamp = -1;

		lethality = 0.0f;
	}

	public void clear()
	{
		flags = 0;

		pos = last_pos = Vector3.zero;
		orient.SetIdentity();
		last_orient.SetIdentity();
	}

	public void create(int type, ref Matrix orient, ref Vector3 pos, float radius, int flags)
	{
		clear();

		this.type = type;

		this.flags = flags | OF_NOT_IN_COLL;

		this.pos = pos;
		this.last_pos = pos;

		this.orient.Copy(ref orient);
		this.last_orient.Copy(ref orient);

		this.radius	= radius;
	}

	/*
	public void AISetGuardVec(Ship _guardShip)
	{
		MathUtils.assert(this != _guardShip);

		if (this == _guardShip) 
		{
			MathUtils.VecRandVecQuick(out guardVec);
			MathUtils.VecScale(ref guardVec, 100.0f);
			return;
		}
		
		float _radius = 5.0f * (radius + _guardShip.radius) + 50.0f;
		if (radius > 300.0f) 
		{
			radius = _guardShip.radius * 1.25f;
		}
		
		MathUtils.VecSub(out guardVec, ref pos, ref _guardShip.pos);
		
		if (MathUtils.VecMag(ref guardVec) > 3.0f * radius) 
		{
			Vector3	tvec, rvec;
			float mag = MathUtils.VecCopyNormalize(out tvec, ref guardVec);
			MathUtils.VecRandVecQuick(out rvec);			
			MathUtils.VecScaleAdd2(ref tvec, ref rvec, 0.5f);
			MathUtils.VecCopyScale(out guardVec, ref tvec, mag);
		}
		
		MathUtils.VecNormalizeQuick(ref guardVec);
		MathUtils.VecScale(ref guardVec, radius);
	}
	*/

	static bool timestamp_elapsed(int stamp) 
	{
		return ((stamp !=0 ) ? (((int)(Time.realtimeSinceStartup * 1000.0f)) >= stamp ? true : false) : false);
	}

	static int timestamp_until(int stamp)
	{
		return stamp - ((int)(Time.realtimeSinceStartup * 1000.0f));
	}

	static void apply_physics(float damping, float desired_vel, float initial_vel, float t, ref float new_vel, ref float delta_pos)
	{
		if (damping < 0.0001f)	
		{
			delta_pos = desired_vel * t;
			new_vel = desired_vel;
		} 
		else 
		{
			float dv = initial_vel - desired_vel;
			float e = (float)Mathf.Exp(-t / damping);

			delta_pos = (1.0f - e) * dv * damping + desired_vel * t;
			new_vel = dv * e + desired_vel;
		}
	}

	void physics_sim_vel(float sim_time)
	{
		Vector3 damp = Vector3.zero;
		
		if (((p_flags & PF_REDUCED_DAMP) > 0) && (timestamp_elapsed(reduced_damp_decay))) 
		{
			p_flags &= ~PF_REDUCED_DAMP;
		}
		
		if ((p_flags & PF_DEAD_DAMP) > 0)
		{
			damp = new Vector3(side_slip_time_const, side_slip_time_const, side_slip_time_const);
		} 
		else if ((p_flags & PF_REDUCED_DAMP) > 0) 
		{
			if (timestamp_elapsed(reduced_damp_decay)) 
			{
				damp = new Vector3(side_slip_time_const, side_slip_time_const, 0.0f);
			} 
			else 
			{
				float reduced_damp_fraction_time_left = timestamp_until(reduced_damp_decay) / (float)REDUCED_DAMP_TIME;

				damp.x = side_slip_time_const * (1 + (REDUCED_DAMP_FACTOR - 1) * reduced_damp_fraction_time_left);
				damp.y = side_slip_time_const * (1 + (REDUCED_DAMP_FACTOR - 1) * reduced_damp_fraction_time_left);
				damp.z = side_slip_time_const * reduced_damp_fraction_time_left * REDUCED_DAMP_FACTOR;
			}
		} 
		else 
		{
			if (use_newtonian_damp) 
			{
				damp = new Vector3(side_slip_time_const, side_slip_time_const, side_slip_time_const);
			} 
			else 
			{
				damp = new Vector3(side_slip_time_const, side_slip_time_const, 0.0f);
			}
		}
	
		Vector3 local_v_in = MathUtils.vm_vec_rotate(ref vel, ref orient);
		Vector3 local_desired_vel = MathUtils.vm_vec_rotate(ref desired_vel, ref orient);

		Vector3 local_disp = Vector3.zero;	
		Vector3 local_v_out = Vector3.zero;	
		
		apply_physics(damp.x, local_desired_vel.x, local_v_in.x, sim_time, ref local_v_out.x, ref local_disp.x);
		apply_physics(damp.y, local_desired_vel.y, local_v_in.y, sim_time, ref local_v_out.y, ref local_disp.y);

		bool special_warp_in = false;
		float excess = local_v_in.z - max_vel.z;

		if (excess > 5 && ((p_flags & PF_SPECIAL_WARP_IN) > 0)) 
		{
			special_warp_in = true;
			float exp_factor = Mathf.Exp(-sim_time / SPECIAL_WARP_T_CONST);

			local_v_out.z = max_vel.z + excess * exp_factor;
			local_disp.z = (max_vel.z * sim_time) + excess * (SPECIAL_WARP_T_CONST * (1.0f - exp_factor));
		} 
		else if ((p_flags & PF_SPECIAL_WARP_OUT) > 0)
		{
			float exp_factor = Mathf.Exp(-sim_time / SPECIAL_WARP_T_CONST);
			Vector3 temp = MathUtils.vm_vec_rotate(ref prev_ramp_vel, ref orient);
			float deficeit = temp.z - local_v_in.z;

			local_v_out.z = local_v_in.z + deficeit * (1.0f - exp_factor);
			local_disp.z = (local_v_in.z * sim_time) + deficeit * (sim_time - (SPECIAL_WARP_T_CONST * (1.0f - exp_factor)));
		} 
		else 
		{
			apply_physics(damp.z, local_desired_vel.z, local_v_in.z, sim_time, ref local_v_out.z, ref local_disp.z);
		}
		
		if (((p_flags & PF_SPECIAL_WARP_IN) > 0) && (excess < 5)) 
		{
			p_flags &= ~(PF_SPECIAL_WARP_IN);
		}
		
		Vector3 world_disp = MathUtils.vm_vec_unrotate(ref local_disp, ref orient);
		pos += world_disp;
		
		vel = MathUtils.vm_vec_unrotate(ref local_v_out, ref orient);
		
		if (special_warp_in) 
		{
			prev_ramp_vel = MathUtils.vm_vec_rotate(ref vel, ref orient);
		}
	}

	void physics_sim_rot(float sim_time)
	{
		float shock_fraction_time_left = 0.0f;
		float shock_amplitude = 0.0f;
		float rotdamp = 0.0f;

		if ((p_flags & PF_IN_SHOCKWAVE) > 0) 
		{
			if (timestamp_elapsed(shockwave_decay)) 
			{
				p_flags &= ~PF_IN_SHOCKWAVE;
				rotdamp = this.rotdamp;
			} 
			else 
			{
				shock_fraction_time_left = timestamp_until(shockwave_decay) / (float)SW_BLAST_DURATION;
				rotdamp = this.rotdamp + this.rotdamp * (SW_ROT_FACTOR - 1) * shock_fraction_time_left;
				shock_amplitude = shockwave_shake_amp * shock_fraction_time_left;
			}
		} 
		else 
		{
			rotdamp = this.rotdamp;
		}

		Vector3 new_vel = Vector3.zero;
		Vector3 delta_pos = Vector3.zero;
		
		apply_physics(rotdamp, desired_rotvel.x, rotvel.x, sim_time, ref new_vel.x, ref delta_pos.x);
		apply_physics(rotdamp, desired_rotvel.y, rotvel.y, sim_time, ref new_vel.y, ref delta_pos.y);
		apply_physics(rotdamp, desired_rotvel.z, rotvel.z, sim_time, ref new_vel.z, ref delta_pos.z);
		
		rotvel = new_vel;

		Angles tangles = new Angles();
		tangles.pitch = rotvel.x * sim_time;
		tangles.heading = rotvel.y * sim_time;
		tangles.bank = rotvel.z * sim_time;

		if ((p_flags & PF_IN_SHOCKWAVE) > 0) 
		{
			tangles.pitch += UnityEngine.Random.Range(-0.5f, 0.5f) * shock_amplitude;
			tangles.heading += UnityEngine.Random.Range(-0.5f, 0.5f) * shock_amplitude;
		}

		last_rotmat = MathUtils.vm_angles_2_matrix(ref tangles);
		Matrix tmp = MathUtils.vm_matrix_x_matrix(ref orient, ref last_rotmat);
		orient.Copy(ref tmp);
		
		MathUtils.vm_orthogonalize_matrix(ref orient);
	}

	void physics_sim(float sim_time)
	{
		if ((p_flags & PF_CONST_VEL) > 0)
		{
			pos += vel * sim_time;
		}
		else
		{
			physics_sim_vel(sim_time);
			physics_sim_rot(sim_time);
			
			speed = vel.magnitude;					
			fspeed = Vector3.Dot(orient.fvec, vel);
		}
	}

	void move_call_physics(float frametime)
	{
		physics_sim(frametime);	
	}

	void accelerate_ship(float accel)
	{
		prev_accel = accel;
	}

	/*
	void ai_turn_towards_vector(ref Vector3 dest, float frametime, float turn_time, ref Vector3? slide_vec, ref Vector3? rel_pos, float bank_override, int flags, ref Vector3? rvec, int sexp_flags)
	{

	}

	void turn_towards_point(ref Vector3 point, ref Vector3? slide_vec, float bank_override)
	{
		ai_turn_towards_vector(point, Time.deltaTime, srotation_time, slide_vec, null, bank_override, 0);
	}
	*/

	object set_target_obj(object obj)
	{
		if (!timestamp_elapsed(ok_to_target_timestamp)) 
		{
			return target_obj;
		}

		if (target_obj == obj) 
		{
			previous_target_obj = target_obj;
		} 
		else 
		{
			previous_target_obj = target_obj;
			
			target_obj = obj;
			target_time = 0.0f;
			time_enemy_near = 0.0f;	
			last_hit_target_time = Missiontime;
		}
		
		return target_obj;
	}

	bool ai_need_new_target()
	{
		if (target_obj == null) return true;

		Ship ship = target_obj as Ship;
		if (ship == null) return true;

		if (ship.team == team) return false;

		//TO DO
		return false;
	}

	void ai_chase()
	{
		//TO DO
	}

	void ai_evade()
	{
		//TO DO
	}

	void ai_guard()
	{
		//TO DO
	}

	void ai_execute_behavior()
	{
		switch (mode) 
		{
		case AIM_CHASE:
			if (En_obj != null) 
			{
				ai_chase();
			} 
			break;

		case AIM_EVADE:
			if (En_obj != null) 
			{
				ai_evade();
			} 
			else 
			{
				Vector3 tvec = pos + orient.rvec * 100.0f;
				//turn_towards_point(tvec, null, 0.0f);
				//accelerate_ship(0.5f);
			}
			break;

		case AIM_GUARD:
			ai_guard();
			break;

		//TO DO

		default:
			break;
		}
	}

	void ai_frame(float frametime)
	{
		p_flags &= ~PF_GLIDING;

		object target_obj = set_target_obj(this.target_obj);

		En_obj = null;

		if (ai_need_new_target()) 
		{
			/*
			target_obj = find_enemy(MAX_ENEMY_DISTANCE, max_attackers[Game_skill_level]);
			if (target_obj != null) 
			{
				if (this.target_obj != target_obj) aspect_locked_time = 0.0f;
				target_obj = set_target_objnum(target_obj);
				
				if (target_obj != null)
				{
					En_obj = target_obj;
				}
			}
			*/
		}
		else if (target_obj != null)
		{
			En_obj = target_obj;
		}

		target_time += frametime;

		ai_execute_behavior();
	}

	void ai_process(float frametime)
	{
		ai_frame(frametime);
	}

	void process_post(float frametime)
	{
		ai_process(frametime);
	}

	void move_post(float frametime)
	{
		process_post(frametime);
	}

	void move(float frametime)
	{
		Vector3 cur_pos = pos;

		last_pos = cur_pos;
		last_orient.Copy(ref orient);

		move_call_physics(frametime);
	
		move_post(frametime);
	}

	// Use this for initialization
	void Start () 
	{
		physics_init();

		Vector3 pos = transform.position;
		Quaternion quat = transform.rotation;

		Matrix orient = new Matrix();
		orient.rvec = quat * Vector3.right;
		orient.uvec = quat * Vector3.up;
		orient.fvec = quat * Vector3.forward;

		float radius = 10.0f;

		create(OBJ_SHIP, ref orient, ref pos, radius, OF_RENDERS | OF_COLLIDES | OF_PHYSICS);

		ai_init();
	}

	// Update is called once per frame
	void Update () 
	{
		Missiontime = (int)(Time.realtimeSinceStartup * 65536);

		move(Time.deltaTime);

		transform.position = pos;
		transform.rotation = Quaternion.LookRotation(orient.fvec, orient.uvec);
	}
}
