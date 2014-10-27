define('views/index',['zepto','sl/sl','app','sl/widget/loading'],function(require,exports,module) {
    var $=require('zepto'),
        sl=require('sl/sl'),
        app=require('app');

    module.exports=sl.Activity.extend({
        template: 'views/index.html',
        onCreate: function() {
            var that=this;

            that.listenResult('addSuccess',function() {
            });

            that.loading=new Loading(that.$el);
        },
        onStart: function() {
        },
        onResume: function() {
        },
        onShow: function() {
            if(!localStorage.getItem('USERINFO')) {
                this.redirect('/login.html');
            } else if(!that.isDataLoad) {
                that.loading.load({
                    url: '/transfer?action=list',
                    success: function() {
                        that.isDataLoad=true;
                    }
                });
            }
        },
        onDestory: function() {
            console.log("index onDestory");
        }
    });
});
