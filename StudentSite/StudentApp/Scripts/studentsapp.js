$(document).ready(function () {
    var StudentsAppViewModel = function() {
        var self = this;
        
        this.students = ko.observableArray([]);
        this.selectedStudent = {
            StudentID: ko.observable(),
            FirstName: ko.observable(),
            LastName: ko.observable(),
            Fees: ko.observableArray([])
        };
        this.selectedStudent.FullName = ko.computed(function () {
            return this.selectedStudent.FirstName() + " " + this.selectedStudent.LastName()
        }, this);

        this.load = function (obj) {
            self.selectedStudent.StudentID(obj.StudentID);
            self.selectedStudent.FirstName(obj.FirstName);
            self.selectedStudent.LastName(obj.LastName);
            self.selectedStudent.Fees(obj.Fees);
        };

        this.paymentMethods = ko.observableArray(["Rank", "Date"])
        this.paymentType = ko.observable("Rank");
        this.amount = ko.observable("0");
        this.showStudent = ko.computed(function () {
            return (self.selectedStudent.StudentID() != null) ? true : false;
        });

        this.pay = function () {
            $.ajax({
                url: 'api/fee',
                type: 'POST',
                data: {
                    StudentID: self.selectedStudent.StudentID(),
                    paymentType: self.paymentType(),
                    amount: self.amount()
                },
                success: function () {
                    window.location.reload();
                }
            });
        }

        $.getJSON('api/students', function (data) {
            for (var i = 0; i < data.length; i++) {
                self.students.push({
                    StudentID:  data[i].StudentID,
                    FirstName:  data[i].FirstName,
                    LastName:   data[i].LastName,
                    Fees:       data[i].fees,
                });
            }
        });
    };
    ko.applyBindings(StudentsAppViewModel());

});

