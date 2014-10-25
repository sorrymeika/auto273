define(['$','sl/sl','ui/tabs','app','ui/loading'],function (require,exports,module) {
    var $=require('zepto'),
        sl=require('sl/sl'),
        app=require('app'),
        Loading=require('ui/loading');

    module.exports=sl.Activity.extend({
        template: 'views/login.html',
        events: {
            'tap .js_back': 'backToFrom',
            'tap .js_login': 'login'
        },

        onCreate: function () {
            var that=this;

        },
        onDestory: function () {
        },

        login: function () {
            var that=this,
                r=this.route.query['r'],
                account=that.$('.js_account').val(),
                password=that.$('.js_password').val();

            if(!account) {
                sl.tip('请填写用户名');
                return;
            }
            if(!password) {
                sl.tip('请填写密码');
                return;
            }

            !that.loading&&(that.loading=new Loading(that.$el));
            that.loading.load({
                url: '/json/user/login',
                success: function (res) {
                    this.hideLoading();
                },
                error: function (res) {
                    this.hideLoading();

                    sl.tip(res.msg);
                }
            });

            //that.redirect(r||'');
        },

        backToFrom: function () {
            this.back();
        }
    });
});
