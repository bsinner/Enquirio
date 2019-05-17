package com.bsinner.enquirio.controller;

import com.bsinner.enquirio.entity.Question;
import com.bsinner.enquirio.entity.User;
import com.bsinner.enquirio.persistence.GenericDAO;
import com.bsinner.enquirio.util.CookieProcessor;
import javax.servlet.RequestDispatcher;
import javax.servlet.ServletException;
import javax.servlet.annotation.WebServlet;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import java.io.IOException;

/**
 * Servlet for creating questions from form data.
 *
 * @author bsinner
 */
@WebServlet("/createQuestion")
public class CreateQuestion extends HttpServlet {

    /**
     * Creates and adds question to DB from request parameters, forwards to page
     * of newly created question.
     *
     * @param req               the HTTP request
     * @param res               the HTTP response
     * @throws IOException      if an I/O Exception occurs
     * @throws ServletException if an Servlet Exception occurs
     */
    protected void doPost(HttpServletRequest req, HttpServletResponse res) throws IOException, ServletException {
        String title = req.getParameter("title");
        String contents = req.getParameter("content");
        // TODO: request validation
        RequestDispatcher dispatcher = req.getRequestDispatcher("/ask.jsp");

        if (title.length() == 0 || contents.length() == 0) {
            if (title.length() == 0) {
                req.setAttribute("title-err", "err");
            }
            if (contents.length() == 0) {
                req.setAttribute("content-err", "err");
            }
            dispatcher.forward(req, res);
            return;
        }

        User user = CookieProcessor.findElseCreateUser(req.getCookies(), res);

        GenericDAO<Question> dao = new GenericDAO<>(Question.class);
        Question question = new Question(title, contents, user);
        int id = dao.insert(question);

        res.sendRedirect(req.getContextPath() + "/question?id=" + id);
    }

}
