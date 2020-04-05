var CommonFunctions = {

      
    parseJsonDates:  function  (jsonDate) {

var MyDate_String_Value = jsonDate;
var value = new Date
            (
                 parseInt(MyDate_String_Value.replace(/(^.*\()|([+-].*$)/g, ''))
            );
var dat = 
          CommonFunctions.addZero(value.getDate()) +
                       "/" +
                       CommonFunctions.addZero(value.getMonth() + 1) +
                       "/" +
      CommonFunctions.addZero(value.getFullYear());


return dat;
    },

    addZero: function (i) {
        if (i < 10) {
            i = "0" + i;
        }
        return i;
    },


}