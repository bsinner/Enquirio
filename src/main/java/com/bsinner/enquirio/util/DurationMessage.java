package com.bsinner.enquirio.util;

import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;
import static java.time.temporal.ChronoUnit.MILLIS;

/**
 * Class for getting time string for UIs.
 *
 * @author bsinner
 */
public class DurationMessage {

    private static final long MIN = 1000 * 60;
    private static final long HOUR = MIN * 60;
    private static final long DAY = HOUR * 24;
    private static final long WEEK = DAY * 7;
    private static final long MONTH = WEEK * 4;

    /**
     * Get a UI friendly message showing how much time has elapsed
     * between now and a given date.
     *
     * @param dateTime the LocalDateTime
     * @return         the elapsed time string
     */
    public static String timeSince(LocalDateTime dateTime) {
        long amount = MILLIS.between(dateTime, LocalDateTime.now());

        if (amount < MIN) {
            return "seconds ago";
        } else if (amount < HOUR) {
            long result = amount / MIN;
            return result +(result == 1 ? " minute ago" : " minutes ago");
        } else if (amount < DAY) {
            long result = amount / HOUR;
            return result +(result == 1 ? " hour ago" : " hours ago");
        } else if (amount < WEEK) {
            long result = amount / DAY;
            return result +(result == 1 ? " day ago" : " days ago");
        } else if (amount < MONTH) {
            long result = amount / WEEK;
            return result +(result == 1 ? " week ago" : " weeks ago");
        }

        return "on " + dateTime.format(DateTimeFormatter.ofPattern("yyyy-MM-dd"));
    }

}
