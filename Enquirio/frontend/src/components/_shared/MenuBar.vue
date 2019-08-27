<template>
    <nav class="navbar navbar-expand-lg navbar-dark bg-dark">

        <router-link to="/" class="navbar-brand text-white">Enquirio</router-link>

        <div class="collapse navbar-collapse justify-content-between">
            
            <ul class="navbar-nav mr-auto">
                <li class="nav-item">
                    <router-link to="/search" class="nav-link">
                        Search Questions
                    </router-link>
                </li>
                <li>
                    <router-link to="/create" class="nav-link">
                        Ask Question
                    </router-link>
                </li>
            </ul>

            <ul class="navbar-nav">
                <li class="nav-item">
                    <router-link to=/signin class="nav-link">Sign In</router-link>
                </li>
                <li>
                    <router-link to="/signup" class="nav-link">Sign Up</router-link>
                </li>
            </ul>

        </div>
    </nav>
</template>