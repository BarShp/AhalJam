namespace AHL.Core.Events
{
    public enum AHLEventNames
    {
        application_load_start = 0,
        application_force_update = 1,
        application_managers_loaded = 2,
        application_system_loaded = 3,
        application_loaded = 4,
        popup_impression = 5,
        screen_change = 6,
        store_item_purchased = 7,
        reward_claimed = 8,
        transaction_complete = 9,
        transaction_started = 10,
        transaction_failed = 11,
        user_levelup = 12,
        clear_block = 13,
        full_block_event = 14,
        back_pressed = 15,
        quit_game = 16,
        pause_game = 17,
        manager_loaded = 18,
        screen_change_start = 19,
    }
}