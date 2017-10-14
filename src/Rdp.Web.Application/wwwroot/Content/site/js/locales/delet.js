    function addPlus(){
    var nameValue = document.getElementById("pushNum").value; 
    var uli= document.getElementById("spand_uli")
    var fc=uli.firstChild
    var li = document.createElement("li");
    li.innerHTML = "<span>" + nameValue + "</span>（10014909）深圳咨询管理系统科";
    uli.insertBefore(li,fc); 
    li.setAttribute("class", "abc");
    var a = document.createElement("a");
    a.setAttribute("class","delt_ul");
    var nameCad = document.getElementById("name_Tt")
    li.appendChild(a);
    a.onclick = function(){
        var trElement = a.parentNode;
        var parentElement = trElement.parentNode;  
       parentElement.removeChild(trElement); 
       return false;
    }
    li.onclick = function(){
        var nameNumP = this.childNodes[0].innerHTML;
       nameCad.innerHTML = nameNumP
    }
}
