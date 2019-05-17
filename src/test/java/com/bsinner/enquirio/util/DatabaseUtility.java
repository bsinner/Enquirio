package com.bsinner.enquirio.util;

import org.apache.logging.log4j.LogManager;
import org.apache.logging.log4j.Logger;

import java.io.BufferedReader;
import java.io.FileNotFoundException;
import java.io.FileReader;
import java.io.IOException;
import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.SQLException;
import java.sql.Statement;
import java.util.ArrayList;
import java.util.Properties;

/**
 * Class to run all sql in a given sql file.
 *
 * @author bsinner
 */
public class DatabaseUtility {

    private final Logger logger = LogManager.getLogger(this.getClass());
    private static final String PROPS_PATH = "/dbUtil.properties";
    private static final char DELIMITER = ';';

    /**
     * Runs the statements in an passed in sql file.
     *
     * @param sql the file to process
     */
    public void runSQL(String sql) {

        Properties properties = loadProperties();

        try (BufferedReader reader = new BufferedReader(new FileReader(sql));
                Connection connection = DriverManager.getConnection(properties.getProperty("jdbc.url"), properties);
                Statement statement = connection.createStatement()
        ) {

            Class.forName(properties.getProperty("jdbc.driver"));

            String query = "";

            while(reader.ready()) {

                char currentChar = (char) reader.read();
                query += currentChar;

                if (currentChar == DELIMITER) {
                    statement.addBatch(query);
                    query = "";
                }
            }

            statement.executeBatch();

        } catch (FileNotFoundException fnf) {
            logger.trace("DatabaseUtility tried to parse an sql file that couldn't be found", fnf);
        } catch (IOException ioe) {
            logger.trace("IOException occurred in DatabaseUtility", ioe);
        } catch (SQLException sqe) {
            logger.trace("SQLException occurred in DatabaseUtility", sqe);
        } catch (ClassNotFoundException cnf) {
            logger.trace("ClassNotFoundException occurred in DatabaseUtility", cnf);
        }
    }

    /**
     * Loads the database utility's properties file.
     *
     * @return the database properties object
     */
    private Properties loadProperties() {
        Properties props = new Properties();

        try {
            props.load(this.getClass().getResourceAsStream(PROPS_PATH));
        } catch (IOException ioe) {
            logger.trace("I/O Exception occurred while loading database properties file", ioe);
        }

        return props;
    }
}
