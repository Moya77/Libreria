// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function formatDate(fecha) {
    
    let dateparts = (fecha.split(" ")[0]).split("/");
    if (dateparts[0].length === 1) {
        dateparts[0] = "0" + dateparts[0];
    }
    if (dateparts[1].length === 1) {
        dateparts[1] = "0" + dateparts[1];
    }
    let newDate = dateparts[2] + "-" + dateparts[1] + "-" + dateparts[0];
    
    return newDate;
}

function GetCustomerByCode() {
    let CodeCustomer = document.getElementById("ClientCode").value;
    fetch("/Ingreso/GetCustomer/" + CodeCustomer + "")
        .then(response => response.json())
        .then(data => {
            document.getElementById("CustomerName").value = data.full_name;

        });
}

function getBook() {
    let bookname = document.getElementById("NombreLibro").value;
    fetch("/Book/GetBook/GetBook/" + bookname + "")
        .then(response => response.json())
        .then(data => {
            document.getElementById("BookCode").value = data.codigo;
            document.getElementById("Empresa").value = data.empresa;
         
        })

}

function getBookByCode() {
    let book_code = document.getElementById("BookCode").value;
    fetch("/Ingreso/GetBook/" + book_code + "")
        .then(response => response.json())
        .then(data => {
            document.getElementById("NombreLibro").value = data.nombre;
        })

}

function getCustomerAdnProducts(IdCliente,index) {

    $.get('/CustomerProductsList/CustomerProductsList?id=' + IdCliente, function (data) {
        $('#modal-body'+index).html(data);
        $('#CustomerProductsModal' + index).modal('show');
    });
    

}

function deleteCustomerProduct(IdCliente, codigo_producto) {

        let deleteData =
        {
        ID_Cliente: IdCliente,
        Codigo_Producto: codigo_producto
        }

    $.post('/Customer/DeleteCustomerProduct', deleteData, function (data) {
        $('#modal-body' + index).html(data);
        $('#CustomerProductsModal' + index).modal('show');
    });


}

function confirmSubmit(event) {
    event.preventDefault();

    Swal.fire({
        title: '¿Desea registrar el libro?',
        showDenyButton: true,
        icon: 'question', 
        showCancelButton: false,
        confirmButtonText: 'Guardar',
        denyButtonText: `No guardar`,
    }).then((result) => {
       
        if (result.isConfirmed) {
            document.getElementById("FormCustomer").submit();
           
        } else if (result.isDenied) {
            Swal.fire({
                title: '¿Desea agregar mas registros?',
                icon: 'info', 
                showDenyButton: true,
                showCancelButton: false,
                confirmButtonText: 'Sí',
                denyButtonText: `No, ir al inicio`,
                
                
            }).then((result) => {

                if (result.isConfirmed) {
                    window.location.reload();

                } else if (result.isDenied) {
                    GoHome();
                }
            })
        }
    })
    return result;
}

function GoHome() {
    window.location.href = homeUrl;
}


$(document).ready(function () {
    $('.modal').modal('show');
});

function confirmSubmitBooks(event) {
    event.preventDefault();

    let table = document.getElementById("TableBooks");
    let date = document.getElementById("OutDate");
    if (table && table.rows.length > 1) {
        if (date.value.trim() !=="") {
            Swal.fire({
                title: '¿Desea realizar el retiro de libros?',
                showDenyButton: true,
                icon: 'question',
                showCancelButton: false,
                confirmButtonText: 'Sí',
                denyButtonText: `No`,
            }).then((result) => {

                if (result.isConfirmed) {
                    document.getElementById("FormCustomerBook").submit();

                } else if (result.isDenied) {
                    Swal.fire('Se a cancelado la operacion!', '', 'info')
                }
            })
        } else {
            Swal.fire('Seleccione la fecha de salida!', '', 'info')  
        }
    } else {
        document.getElementById("FormCustomerBook").submit();
       
    }
    return result;
}