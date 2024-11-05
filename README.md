# Securing passwords using k8 secrets

Only the root user on the linux machine where the k8 cluster running this app is hosted can access
[this secret password](https://github.com/tushardevsharma/very-secure-app/blob/69cc1d5dcc48f9055a8dfe25adbd5ba5f8d922bc/k8s/deploy.yml#L22-L26).
Nowhere in source code or in config file is the password available (either in plain text or in encoded form). But the app can still securely
[access it](https://github.com/tushardevsharma/very-secure-app/blob/69cc1d5dcc48f9055a8dfe25adbd5ba5f8d922bc/VerySecureApp.API/Program.cs#L42).
