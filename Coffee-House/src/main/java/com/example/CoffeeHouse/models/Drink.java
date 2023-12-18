package com.example.CoffeeHouse.models;

import com.example.CoffeeHouse.models.enums.Size;
import jakarta.persistence.*;
import lombok.Getter;
import lombok.Setter;

import java.util.Date;
import java.util.HashSet;
import java.util.Set;


@Entity
@Table(name = "drinks")
public class Drink {
    @Getter
    @Setter
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;

    @Getter
    @Setter
    @Column(nullable = false)
    private String name;

    @Getter
    @Setter
    @Column(nullable = false)
    @Enumerated(EnumType.STRING)
    private Size size;

    @Getter
    @Setter
    @Column(nullable = false)
    private double price;

    @Getter
    @Setter
    @ManyToOne(fetch = FetchType.LAZY)
    @JoinColumn(name = "recipeId", referencedColumnName = "id", nullable = true)
    private Recipe recipe;

    @Getter
    @Setter
    @ManyToOne(fetch = FetchType.LAZY)
    @JoinColumn(name = "categoryId", referencedColumnName = "id", nullable = true)
    private Category category;

}

