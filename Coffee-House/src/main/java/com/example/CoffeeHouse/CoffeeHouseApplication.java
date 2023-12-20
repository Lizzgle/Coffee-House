package com.example.CoffeeHouse;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;

import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.ResultSet;
import java.sql.Statement;

@SpringBootApplication
public class CoffeeHouseApplication {

	public static void main(String[] args) {
		String url = "jdbc:postgresql://localhost:5432/Coffee-house";
		String user = "postgres";
		String password = "3851";

		try (Connection connection = DriverManager.getConnection(url, user, password)) {
			System.out.println("Connected to the PostgreSQL server successfully.");

			Statement statement = connection.createStatement();
			ResultSet resultSet = statement.executeQuery("SELECT * FROM Users");

			while (resultSet.next()) {
				System.out.println("ID: " + resultSet.getInt("Id") +
						", Login: " + resultSet.getString("Login") +
						", Name: " + resultSet.getString("Name"));
			}
		} catch (Exception e) {
			System.out.println("Connection failed.");
			e.printStackTrace();
		}

		SpringApplication.run(CoffeeHouseApplication.class, args);
	}

}
