# Checklist de Progreso — Obligatorio 2026 (StellarMinds)

> Estado al **2026-06-09**. Leyenda: ✅ hecho y verificado · ⚠️ hecho parcial / revisar · ❌ falta · 🙋 depende de vos (cuenta, aula, fotos, etc.)

---

## Varios

| Estado | Ítem | Nota |
|---|---|---|
| ✅ | Datos de prueba (scripts SQL e inserts) | Creado `StellarMinds - Server/Scripts/StellarMinds_DatosDePrueba.sql`. **Ejecutado y verificado** contra `StellarMindsDb`. Re-ejecutable (limpia + reinserta con IDs fijos). |
| ✅ | Al menos 10 registros por tabla | Verificado en BD: **Usuarios 12 · Equipos 16 · ObjetosCelestes 12 · Prestamos 12 · Observaciones 12 · Auditorias 12**. |
| ✅ | Implementado en .NET 10, EF Core 10, MVC y Web API, HttpClient | Verificado en código |
| ✅ | Swagger configurado y operativo con documentación | `Program.cs` con `AddSwaggerGen` + seguridad JWT |
| 🙋 | Evidencia de pruebas de la API con Swagger o Postman | Hay que sacar capturas/colección |
| ✅ | Repositorio GitHub | Pusheado a `github.com/FabricioFerreira1234/Obligatorio2026-Fabricio` (historial limpiado de la API key de Gemini). |
| 🙋 | Despliegue API+BD en SOMEE (si grupo de 2) / local + MVC consume SOMEE | Depende del tamaño del grupo |
| ⚠️ | Realizar precarga de datos con ayuda de ChatGPT | Hecho vía seed; documentar que se usó IA |
| 🙋 | Pre-entregas en aulas | Gestión del curso |

## Documentación

| Estado | Ítem | Nota |
|---|---|---|
| ❌ | PDF único | A compilar al final |
| 🙋 | Archivo Astah incluido | Requiere Astah (no se puede generar desde código) |
| 🙋 | Carátula, integrantes, fotos y tabla de contenido | Necesito datos de integrantes/fotos |
| ❌ | Documenta uso de IA generativa | Redactar sección |
| ❌ | Caso de uso narrativo RF07 (Alta de observación) | Redactar |
| ❌ | Caso de uso narrativo RF08 (Listado de préstamos entre fechas) | Redactar |
| ❌ | Casos de prueba RF10 (Ranking de objetos celestes) | Redactar |
| 🙋 | Diagrama de casos de uso | Astah |
| 🙋 | Diagrama de clases completo de lógica de negocio | Astah |
| 🙋 | Diagrama de clases de RF07 para demás capas | Astah |
| ❌ | Documenta generación de datos con IA | Redactar |
| ❌ | Documenta investigación API Gemini | Redactar (RF07 usa Gemini con fallback local) |

## Arquitectura / Diseño / Estilo

| Estado | Ítem | Nota |
|---|---|---|
| ✅ | Controladores Web API consumen casos de uso mediante DTOs | Verificado |
| ✅ | Casos de uso utilizan repositorios y mappers | Verificado |
| ✅ | Capa de datos utiliza EF Core | `LogicaAccesoDatos/EF` |
| ✅ | Uso de DTOs, entidades y Value Objects | `FechaVO`, `DireccionVO`, `NombreCompletoVO` |
| ⚠️ | Aplica principios SOLID | Revisión cualitativa pendiente |
| ✅ | Aplica Arquitectura Limpia | Capas separadas |
| ✅ | Excepciones personalizadas para validaciones | `EquipoException`, `PrestamoException`, etc. |
| ⚠️ | Manejo adecuado de errores | `RespuestaApi.LeerError`; revisar cobertura |
| ✅ | Dos soluciones: una MVC y otra Web API | `StellarMinds.slnx` (server) + `StellarMinds.WebApp.slnx` (cliente) |
| ⚠️ | Utiliza DataAnnotations, por lo menos algunas | Confirmar en DTOs/ViewModels |
| ⚠️ | Uso exclusivo de LINQ para consultas | Confirmar repositorios |

## BackEnd — API RestFull

| Estado | Ítem | Nota |
|---|---|---|
| ✅ | API usa JWT para endpoints que requieren autenticación | `AddJwtBearer` + `[Authorize]` |
| ⚠️ | Códigos de retorno correctos | Revisar por controlador |
| ✅ | Cadena de conexión en appsetting.json | `DefaultConnection` |
| ⚠️ | Cumple reglas de negocio descriptas | Revisar por RF |

## Front — una solución MVC con HttpClient

| Estado | Ítem | Nota |
|---|---|---|
| ✅ | Utiliza el API en forma adecuada | `ClienteHttpAuxiliar` |
| ✅ | Guarda el token JWT en session | `HttpContext.Session.GetString("token")` |

---

## Requerimientos Funcionales

| RF | Descripción | Server (CU/Ctrl) | Cliente (Ctrl/View) | Estado |
|---|---|---|---|---|
| RF01 | Login / Logout | `CULogin`, `LoginController` | `LoginController`, `Login/Index` | ✅ |
| RF02 | Alta de usuarios (Admin) | `CUAltaUsuario`, `UsuarioController` | `Usuario/Create` | ✅ |
| RF03 | CRUD de equipos (Admin) | `CUAlta*/CUEditar*/CUBaja`, `EquipoController` | `Equipo/*` | ✅ |
| RF04 | Alta de préstamo (Coordinador) | `CUAltaPrestamo` | `Prestamo/Create` | ✅ |
| RF05 | Devolución de préstamo (Coordinador) | `CUDevolverPrestamo` | `Prestamo/Devolucion` | ✅ |
| RF06 | Auditoría automática | `Auditoria` entidad + repos | (automático) | ✅ |
| RF07 | Alta de observación (Socio) + Gemini | `CUAltaObservacion`, `CUEvaluarAdecuacion` | `Observacion/Create` | ✅ |
| RF08 | Listado de préstamos entre fechas (Socio) | `CUListadoPrestamosSocio` | `Prestamo/MisPrestamos` | ✅ |
| RF09 | Socios por telescopio (Admin/Coord) | `CUSociosPorTelescopio` | `Prestamo/SociosPorTelescopio` | ✅ |
| RF10 | Ranking de objetos celestes (cualquier rol) | `CURankingObjetosCelestes` | `Ranking/Index` | ✅ |
| RF11 | Información de auditoría (Admin) | `CUAuditoriaPrestamo`, `CUDetallePrestamo` | `Auditoria/*` | ✅ commiteado y pusheado |

---

## Próximos pasos sugeridos (lo que SÍ puedo hacer con vos)

1. **Script SQL de datos de prueba** (`inserts`) — entregable faltante.
2. **Completar 10 registros por tabla** (Telescopios, Monturas, Cámaras, Oculares, Préstamos, Observaciones).
3. **Redactar la documentación** (casos de uso narrativos RF07/RF08, casos de prueba RF10, uso de IA, investigación Gemini) → puedo dejarla en Word.
4. **Verificar y commitear RF11** (Auditoría) que está sin guardar en git.
5. **Confirmar push a GitHub.**

> Lo marcado con 🙋 (Astah, fotos, cuenta SOMEE, pre-entregas) lo tenés que hacer vos; te puedo guiar.
