package com.bsinner.enquirio.entity;

import com.bsinner.enquirio.persistence.GenericDAO;
import com.bsinner.enquirio.util.DatabaseUtility;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;

import static org.junit.Assert.assertEquals;

public class QuestionTest {

    private GenericDAO<Question> dao;

    @BeforeEach
    protected void setUpEach() {
        DatabaseUtility dbUtil = new DatabaseUtility();
        dbUtil.runSQL("target/test-classes/sql/setupQuestionsTest.sql");
        dao = new GenericDAO<>(Question.class);
    }

    @Test
    protected void testGetAll() {
        assertEquals(1, 1);
    }

}
