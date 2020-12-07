namespace ApplicationCore
{
    public enum UserPlanType
    {
        Basic = 1, 
        Licensed = 2,
        OnPrem = 3
    }

    public enum ZoomMeetingType
    {
        Instant = 1, 
        Scheduled = 2,
        RecurringWithNoFixedTime = 3,
        RecurringWithFixedTime = 8
    }

    public enum ZoomMeetingApproval
    {
        Automatic = 0,
        Manual = 1,
        NoRegRequired = 2
    }

    public enum ZoomMeetingRegistrationType
    {
        Attendees_register_once_and_can_attend_any_of_the_occurrences = 1,
        Attendees_need_to_register_for_each_occurrence_to_attend = 2,
        Attendees_register_once_and_can_choose_one_or_more_occurrences_to_attend = 3
    }
}
