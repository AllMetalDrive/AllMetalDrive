
# AllMetalDrive
Proyecto final Manejo de Proyectos

# **All Metal Drive**

### **Boss Rush Shooter con Elementos de Deckbuilding · Roguelike · Gacha**

**All Metal Drive** es un shooter frenético centrado en combates contra jefes, progresión roguelike y sistemas de personalización basados en Gacha y Deckbuilding.
El proyecto se desarrolla en **Unity**.

---

## **Concepto del Juego**

El mundo de **All Metal Drive** gira en torno al choque entre **naturaleza** y **corrupción tecnológica**.
El jugador se enfrenta a enemigos mecánicos y cibernéticos dentro de escenarios que reflejan esta dualidad.

---

## **Entorno de Desarrollo**

### **Motor y Versión**

Para garantizar compatibilidad entre todos los colaboradores:

* **Motor:** Unity
* **Versión:** **6000.2.9f1**

---

## **Guía de Estilo Rápida (C#)**

La coherencia del código es obligatoria.
Todo el código debe escribirse en **inglés** y seguir convenciones estándar de C#.

### **Convenciones de Nomenclatura**

| Elemento                | Convención                 | Ejemplo Correcto                      |
| ----------------------- | -------------------------- | ------------------------------------- |
| **Clases / Archivos**   | PascalCase                 | `PlayerController.cs`                 |
| **Funciones / Métodos** | PascalCase                 | `StartDash()`                         |
| **Variables públicas**  | camelCase                  | `public float moveSpeed;`             |
| **Variables privadas**  | camelCase con `_` opcional | `private bool _isDashing;`            |
| **Constantes**          | ALL_CAPS_SNAKE_CASE        | `private const float MAX_JUMP_HEIGHT` |

### **Comentarios y Organización de Scripts**

* Cada script debe comenzar con un **encabezado general** (autor, fecha, descripción, secciones).
* Todas las variables serializadas deben agruparse usando `[Header("GRUPO")]`.
* Debajo de cada `Header`, cada variable debe tener un comentario breve explicando su propósito.

---

## **Flujo de Trabajo (Git Flow)**

Usamos **Git Flow** para mantener un repositorio ordenado, escalar el trabajo colaborativo y manejar releases controlados.

### **Ramas Principales**

* `main` — Rama **estable** y lista para producción.
* `develop` — Rama de **integración** donde se combinan las features.
* `feature/<nombre>` — Nuevas funcionalidades (deriva de `develop`).
* `bugfix/<nombre>` — Correcciones durante desarrollo (deriva de `develop`).
* `hotfix/<nombre>` — Arreglos críticos sobre `main` (merge hacia `main` y `develop`).

---

### **Regla Principal**

### **Nadie trabaja directamente en `develop` ni en `main`.**

Siempre debes:

1. Crear tu rama propia (`feature/*`, `bugfix/*`, etc).
2. Abrir un **Pull Request (PR)** hacia `develop`.

---


## **Flujo de Trabajo Rápido**

##  **Flujo de Trabajo Rápido**


### 1️ Sincronizar `develop`

```bash
git checkout develop
git pull origin develop
```

### 2️ Crear tu rama de trabajo

```bash
git checkout -b feature/nueva-funcionalidad
```

### 3️ Commits y push

```bash
git push origin feature/nueva-funcionalidad
```

### 4️ (Opcional) Mantener tu rama actualizada

```bash
git checkout feature/nueva-funcionalidad
git merge develop
# o git rebase develop (historial lineal)
```

### 5️ Crear el Pull Request hacia `develop` en GitHub.

---

##  **Checklist del Pull Request**

Tu PR debe cumplir con:

* La rama fue creada correctamente desde `develop`.
* La rama está actualizada con `develop`.
* La descripción del PR explica claramente los cambios.
* Incluye instrucciones de prueba.
* El proyecto compila correctamente.
* Tiene al menos **1–2 aprobaciones**.
* Los checks de CI (build, tests, linter) pasan.

---

## **Seguimiento de Tareas**

Utilizaremos la siguiente hoja de cálculo para gestión de tareas, estado y fechas de entrega:

> **ENLACE AL EXCEL**
> **[https://docs.google.com/spreadsheets/d/1NK7lU8IotFBW8ii7bY393KrzCr3xhu_KCtaRawwzVlY/edit?gid=0#gid=0 ]**


