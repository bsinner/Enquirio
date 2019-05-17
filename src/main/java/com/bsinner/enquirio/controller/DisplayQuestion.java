package com.bsinner.enquirio.controller;

import com.bsinner.enquirio.entity.Question;
import com.bsinner.enquirio.persistence.GenericDAO;
import com.bsinner.enquirio.util.DurationMessage;
import com.bsinner.enquirio.util.UserAccessTokenParser;

import javax.servlet.RequestDispatcher;
import javax.servlet.ServletException;
import javax.servlet.annotation.WebServlet;
import javax.servlet.http.Cookie;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import java.io.IOException;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

/**
 * Servlet for displaying a single question.
 *
 * @autor bsinner
 */
@WebServlet(urlPatterns = "/question")
public class DisplayQuestion extends HttpServlet {

    private static final Pattern REGEX = Pattern.compile("^.*id=([0-9]+).*$");

    /**
     * Forward to a page with the question described in the query string's details
     * or forward to a 404 page if no question was found.
     *
     * @param req               the HTTP Request
     * @param res               the HTTP Response
     * @throws IOException      if an I/O Exception is thrown
     * @throws ServletException if a Servlet Exception is thrown
     */
    public void doGet(HttpServletRequest req, HttpServletResponse res) throws IOException, ServletException {

        int id = getId(req.getQueryString());
        RequestDispatcher dispatcher;

        if (id != 0) {
            Question q = getQuestion(id);

            if (q != null) {
                req.setAttribute("question", q);
                req.setAttribute("timeSince", getTimeSense(q));
                dispatcher = req.getRequestDispatcher("/question.jsp");
            } else {

                dispatcher = req.getRequestDispatcher("/questionNotFound.jsp");

            }

        } else {
            dispatcher = req.getRequestDispatcher("/questionNotFound.jsp");
        }


        dispatcher.forward(req, res);
    }

    private int getId(String queryString) {

        if (queryString != null) {
            Matcher matcher = REGEX.matcher(queryString);
            if (matcher.find()) {
                return Integer.valueOf(matcher.group(1));
            }
        }

        return 0;
    }

    private Question getQuestion(int id) {
        GenericDAO<Question> dao = new GenericDAO<>(Question.class);
        return dao.getById(id);
    }

    private String getTimeSense(Question question) {
        // TODO: check if question is edited
        return DurationMessage.timeSince(question.getCreatedDate());
    }

}
