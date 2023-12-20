package com.example.CoffeeHouse.models;

import jakarta.persistence.*;
import java.util.Date;
import java.util.Set;

@Entity
@Table(name = "Users")
public class User {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;

    @Column(name = "Login", unique = true, nullable = false)
    private String login;

    @Column(name = "Password", nullable = false)
    private String password;

    @Column(name = "Name", unique = true, nullable = false)
    private String name;

    @Column(name = "DateOfBirth")
    private Date dateOfBirth;

    @ManyToMany
    @JoinTable(name = "UsersCoupons",
            joinColumns = @JoinColumn(name = "UserId"),
            inverseJoinColumns = @JoinColumn(name = "CouponId"))
    private Set<Coupon> coupons;

    public void setId(Long id) {
        this.id = id;
    }

    public Long getId() {
        return id;
    }

}