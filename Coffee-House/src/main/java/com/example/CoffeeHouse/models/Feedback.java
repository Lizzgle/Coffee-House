package com.example.CoffeeHouse.models;

import jakarta.persistence.*;
import java.util.Date;
import java.util.Set;

@Entity
public class Feedback {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;

    @Column(nullable = false)
    private Date date;

    @Column(nullable = false)
    private int rating;

    private String description;

    @ManyToOne
    @JoinColumn(name = "UserId", referencedColumnName = "Id")
    private User user;

    // Геттеры и сеттеры
}