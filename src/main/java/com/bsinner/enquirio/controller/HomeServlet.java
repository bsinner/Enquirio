package com.bsinner.enquirio.controller;

import com.bsinner.enquirio.entity.Question;
import com.bsinner.enquirio.persistence.GenericDAO;

import javax.servlet.RequestDispatcher;
import javax.servlet.ServletException;
import javax.servlet.annotation.WebServlet;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import java.io.IOException;

/**
 * Servlet that fetches data and forwards to index.jsp.
 *
 * @author bsinner
 */
@WebServlet(urlPatterns = "/home")
public class HomeServlet extends HttpServlet {

    /**
     * Fetch all questions and forward to index.jsp.
     *
     * @param req               the HTTP Request
     * @param res               the HTTP Response
     * @throws ServletException if a Servlet Exception occurs
     * @throws IOException      if an I/O Excpetion occurs
     */
    @Override
    protected void doGet(HttpServletRequest req, HttpServletResponse res) throws ServletException, IOException {
        GenericDAO<Question> dao = new GenericDAO<>(Question.class);

        req.setAttribute("questions", dao.getAll());

        RequestDispatcher dispatcher = req.getRequestDispatcher("/index.jsp");
        dispatcher.forward(req, res);
    }

}
