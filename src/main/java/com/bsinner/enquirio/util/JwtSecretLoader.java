package com.bsinner.enquirio.util;

import org.apache.logging.log4j.LogManager;
import org.apache.logging.log4j.Logger;
import java.io.*;
import java.net.URISyntaxException;

/**
 * Class for loading the current JWT secret used to sign tokens.
 *
 * @author bsinner
 */
public class JwtSecretLoader {

    private final Logger logger = LogManager.getLogger(this.getClass());
    private static final String PATH = "/tokenSecret.txt";

    /**
     * Get the current secret.
     *
     * @return the secret in String form
     */
    public String getSecret() {
        String results = "";

        try (BufferedReader reader = new BufferedReader(new FileReader(getFile()))) {
            results = reader.readLine();
        } catch (FileNotFoundException fnf) {
            logger.trace("JWT Secret file was not found", fnf);
        } catch (IOException ioe) {
            logger.trace(ioe);
        }

        return results;
    }

    /**
     * Get the file that stores the secret
     *
     * @return the secret file
     */
    private File getFile() {
        File file = null;

        try {
            file = new File(this.getClass().getResource(PATH).toURI());
        } catch (URISyntaxException uri) {
            logger.trace(uri);
        }

        return file;
    }
}
