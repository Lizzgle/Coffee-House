package com.example.CoffeeHouse.models;

import jakarta.persistence.*;
import java.util.Date;
import java.util.Set;

@Entity
@Table(name = "Coupons")
public class Coupon {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;

    @Column(name = "Discount", nullable = false)
    private Integer discount;

    @Column(name = "dateofend", nullable = false)
    private Date dateOfEnd;

    @ManyToMany(mappedBy = "coupons")
    private Set<User> users;

    // Геттеры и сеттеры
}