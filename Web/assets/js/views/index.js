define('views/index',['zepto','sl/sl','app'],function (require,exports,module) {
    var $=require('zepto'),
        sl=require('sl/sl'),
        app=require('app');

    module.exports=sl.Activity.extend({
        template: 'views/index.html',
        onCreate: function () {
            var that=this;
        },
        onStart: function () {
        },
        onResume: function () {
            console.log("index onResume");
        },
        onShow: function () {
            if(!localStorage.getItem('USERINFO')) {
                this.redirect('/login.html');
            }
        },
        onDestory: function () {
            console.log("index onDestory");
        }
    });
});
