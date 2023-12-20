package com.example.CoffeeHouse.controllers;

import org.springframework.web.bind.annotation.GetMapping;

public class MainController {
    @GetMapping("/")
    public String securityURL() {
        return "home";
    }
}
