package com.bsinner.enquirio.controller;

import javax.servlet.RequestDispatcher;
import javax.servlet.ServletException;
import javax.servlet.annotation.WebServlet;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import java.io.IOException;

/**
 * The ask question servlet.
 *
 * @author bsinner
 */
@WebServlet(urlPatterns = "/ask")
public class AskQuestionServlet extends HttpServlet {

    /**
     * Forwards to page with ask question form.
     *
     * @param req               the HTTP request
     * @param res               the HTTP response
     * @throws ServletException if a servlet exception occurs when forwarding to ask.jsp
     * @throws IOException      if an I/O exception occurs when forwarding to ask.jsp
     */
    @Override
    protected void doGet(HttpServletRequest req, HttpServletResponse res) throws ServletException, IOException {
        RequestDispatcher rd = req.getRequestDispatcher("/ask.jsp");
        rd.forward(req, res);
    }

}
