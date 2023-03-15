# enovaApi.Proxy

Soneta.WebApi nie umożliwia autoryzacji za pomocą klucza API, aby w komunikatach do enovy nie wpisywać użytkownika i hasła można skorzystać z przygotowanego proxy.
W tym celu w appsettings.json konfigurujemy kestrel oraz podajemy adres api enovy, a następnie instalujemy usługę.

Aby wygenerować klucz do szyfrowania należy uruchomić aplikacje z parametrem _--generateCrypto_
![image](https://user-images.githubusercontent.com/87368964/225439256-76b27d44-cd9a-4854-b861-d1049de99001.png)

Uzyskane wartości uzupełniamy w pliku _appsettings.json_ w parametrach _"IV"_ oraz _"Key"_

Aplikacja wystawia 2 endpointy:
- http://localhost:7065/api/Keys - do tworzenia kluczy, w parametrach podajemy nazwę użytkownika i hasło.
Dla jednej nazwy użytkownika możemy wygenerować tylko 1 klucz. W celu ponownego wygenerowania klucza w parametrach należy dodać _regenerate=true_.
W odpowiedzi dostajemy klucz api, który wraz z użytkownikiem i hasłem, przechowywane są w pliku keys.json (hasło jest zaszyfrowane)
![image](https://user-images.githubusercontent.com/87368964/221992553-adcf8772-3565-4b6f-8411-eead0f560dc5.png)
- http://localhost:7065/api/MethodInvoker/InvokeServiceMethod - do wywoływania api enovy
![image](https://user-images.githubusercontent.com/87368964/221992496-0a7c5a3c-aa17-4164-b7f9-315f03c91803.png)
