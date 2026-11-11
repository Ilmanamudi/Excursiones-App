---

## 🛡️ Sistema de Gestión Excursiones

La aplicación implementa un sistema de control de acceso basado en roles (RBAC) flexible y seguro mediante **Claims** personalizados y un filtro de autorización por acciones (`[TieneAccion]`).

### 👥 Roles del Sistema
1. **Administrador (`RolAdmin`)**: Posee privilegios de gestión global sobre el catálogo de excursiones, estados y supervisión de reservas totales. Tiene prohibido realizar reservas personales de pasajeros.
2. **Cliente (`RolCliente`)**: Cuenta con acceso exclusivo al catálogo público, creación de reservas propias, modificación, cancelación y calificación de experiencias finalizadas.

### ⚙️ Asignación de Roles y Permisos
El primer usuario registrado toma el rol de ADMIN, los demas usuarios registrados son clientes. 

### ⚙️ Test de funcionalidades
Al ejecutar Excursiones_y_reservas.sql se ingresan datos a la base para agilizar el test de las funcionalidades.
