# Diseño del Sistema de Gestión de Facturación

Este `README.md` describe la estructura de un sistema de gestión de facturación, basado en un diagrama de clases UML y requisitos adicionales de un documento PDF. El objetivo es modelar entidades como `Persona`, `Cliente`, `Factura` e `Item`, así como definir las operaciones de negocio, validaciones y la estructura del menú de usuario.

## 1. Visión General del Diseño

El sistema está estructurado alrededor de varias entidades y sus interacciones, utilizando interfaces para definir contratos y herencia para reutilizar comportamiento.

```mermaid
classDiagram
    class IPersona {
        +GetCuit(d): Cuit/Cuil
        +GetRazonSocial(d): RazonSocial
        +GetDomicilio(d): Domicilio
        -Baja()
        -Modificacion()
        -Listado()
    }

    class IFactura {
        -GetTipoFactura(): Tipo
        -GetNroFactura(): Numero
        -GetFechaFactura(): Fecha
        -GetImporteFactura(): Importe
        -VistaPreviaFactura()
        -EmitirFactura()
        -Consulta(d): Factura
    }

    class IItem {
        -GetDescripcion(): Descripcion
        -GetCantidad(): Cantidad
        -GetImporte(): Importe
    }

    class AbstractCrud {
        +abstract Alta()
        +virtual Modificacion()
        +abstract Consulta()
        +virtual Listado()
    }

    class Cliente {
        +ID
        +Cuit/Cuil
        +RazonSocial
        +Domicilio
        +List<Factura> Facturas
        --
        +GetCuit(d): Cuit/Cuil
        +GetRazonSocial(d): RazonSocial
        +GetDomicilio(d): Domicilio
        +Alta(id,cuit,razonsocial,domicilio)
        +Baja(id)
        +Modificacion(id)
        +Consulta(id)
        +Listado()
    }

    class ICrudFactura {
        +Alta()
        +Consulta()
    }
    
    class Factura {
        +ID
        +Tipo
        +Numero
        +Fecha
        +ImporteTotal
        +List<Item> Items
        +FK_ClienteID
        --
        +GetTipoFactura(d): Tipo
        +GetNroFactura(d): Numero
        +GetFechaFactura(d): Fecha
        +EmitirFactura()
        +Alta(id)
        +Consulta(id): Factura
    }

    class Item {
        +ID
        +Descripcion
        +Cantidad
        +Importe
        +FK_FacturaID
        --
        +GetDescripcion(d): Descripcion
        +GetCantidad(d): Cantidad
        +GetImporte(d): Importe
    }

    class Validaciones {
        +ValidarCliente()
        +ValidarCuit()
        +ValidarRazonSocial()
        +ValidarDomicilio()
        +ValidarFactura()
        +ValidarTipo()
        +ValidarNumero()
        +ValidarFecha()
        +ValidarItem()
        +ValidarDescripcion()
        +ValidarCantidad()
        +ValidarImporte()
    }

    class Menu {
        -LimpiarPantalla()
    }

    IPersona <|-- Cliente
    AbstractCrud <|-- Cliente
    ICrudFactura <|-- Factura
    IFactura <|-- Factura
    IItem <|-- Item
    
    Cliente "1" -- "N" Factura : tiene
    Factura "1" -- "N" Item : contiene
