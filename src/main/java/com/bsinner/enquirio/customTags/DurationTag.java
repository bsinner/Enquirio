package com.bsinner.enquirio.customTags;

import javax.servlet.jsp.JspWriter;
import javax.servlet.jsp.tagext.SimpleTagSupport;
import java.io.IOException;
import java.time.LocalDateTime;

public class DurationTag extends SimpleTagSupport {
    private LocalDateTime dateTime;

    public void setDateTime(LocalDateTime dateTime) {
        this.dateTime = dateTime;
    }

    public void doTag() throws IOException {

        if (dateTime != null) {
            JspWriter writer = getJspContext().getOut();
//            writer.println(dateTime.toString());
            writer.println("HELLO WORLD");
        }
    }
}
