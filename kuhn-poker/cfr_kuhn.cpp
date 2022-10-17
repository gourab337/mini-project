#include<bits/stdc++.h>

using namespace std;

vector<string> Actions = {"B","C"}; // bet/call vs check/fold

//sum function
float sum(vector<float> &arraySum){
    float s=0;
    for(int i=0;i<arraySum.size();i++){
        s+=arraySum[i];
    }
    return s;
}

class InformationSet{
    
    public:
        vector<float> cumulative_regrets;
        vector<float> strategy_sum;
        int num_actions;
        InformationSet(){
            for(int i=0;i<Actions.size();i++){
                cumulative_regrets.push_back(0.0);
                strategy_sum.push_back(0.0);
            }
            num_actions = Actions.size();
        }

        vector<float> normalize(vector<float> &strategy){
            // Normalize a strategy. If there are no positive regrets, use a uniform random strategy
            if (sum(strategy) > 0) {
                float val=sum(strategy);
                for(int i=0;i<strategy.size();i++)strategy[i]/=val;
            }
            else {
                // for i -> num_actions
                
                for(int i=0;i<num_actions;i++)strategy[i]=1/(num_actions*1.0);
            }
            return strategy;
        }

        vector<float> get_strategy(float reach_probability) {
            // Return regret-matching straategy
            // for i -> len strategy .. .. 
            vector<float>strategy;
            float zero=0.0;
            //cumulative_regrets and strategy_sum is not here in function
            for(int i=0;i<cumulative_regrets.size();i++)strategy.push_back(max(cumulative_regrets[i],zero));
            strategy=normalize(strategy);
            for(int i=0;i<strategy_sum.size();i++)strategy_sum[i]+=reach_probability*strategy[i];

            return strategy;
        }

        vector<float> get_average_strategy(){
            vector<float>copy_strategy_sum;
            for(int i=0; i<strategy_sum.size(); i++) {
                copy_strategy_sum.push_back(strategy_sum[i]);
            }
            return normalize(copy_strategy_sum);
        }

};

class KuhnPoker {
    public:
    bool is_terminal(string history) {
        vector<string> historyList = {"BC", "BB", "CC", "CBB", "CBC"};
        for(auto itr: historyList) {
            if (history == itr) {
                return true;
            }
            else {
                return false;
            }
        }
    }

    int get_payoff(string history, vector<string> cards) {
        // get payoff for 'active' player in terminal history
        vector<string> historyList = {"BC", "CBC"};
        for(auto itr: historyList) {
            if (history == itr) {
                return +1;
            }
            else { // CC or BB or CBB
                float payoff=1;
                for (int i=0; i< history.length(); i++){
                    if('B'==history[i]){
                        payoff = 2;
                        break;
                    }
                }
                int active_player = history.length() %2;
                
                string player_card = cards[active_player];
                string opponent_card = cards[(active_player + 1) % 2];
                
                if(player_card == "K" || opponent_card == "J") {
                    return payoff;
                }
                else {
                    return -payoff;
                }

            }
        }
    }

};

class KuhnCFRTrainer {

    public: 
        map<string,InformationSet> infoset_map;
        

        InformationSet get_information_set(string card_and_history){
            // add if needed and return 
            int flag = 1;
            for(auto itr: infoset_map) {
                if(itr.first == card_and_history) {
                    flag = 0;
                    break;
                }
            }
            if(flag){
                infoset_map[card_and_history] = InformationSet();
            }
            return infoset_map[card_and_history];
        }

        float cfr(vector<string> cards, string history, vector<float> reach_probabilities, int active_player){
            KuhnPoker obj;
            if(history.length()>3)return 0;
            // cout<<history<<endl;
            if( obj.is_terminal(history)){
                return obj.get_payoff(history, cards);
            }
            string my_card;
            my_card = cards[active_player];
            // check if correct
            InformationSet info_set;
            info_set = get_information_set(my_card+history);
            vector<float> strategy;
            // strategy = {1,0};
            strategy = info_set.get_strategy(reach_probabilities[active_player]);
            int opponent;
            opponent = (active_player + 1) % 2 ; 

            vector<float> counterfactual_values;
            for(int i=0;i<Actions.size();i++)counterfactual_values.push_back(0.0);

            float action_probability;
            vector<float>new_reach_probabilities;
            for(int i=0; i< Actions.size(); i++){
                
                action_probability = strategy[i];

                // compute new reach probabilities after this action
                // new_reach_probabilities = reach_probabilities.copy()
                for(int i=0; i<reach_probabilities.size(); i++) {
                    new_reach_probabilities.push_back(reach_probabilities[i]);
                }
                new_reach_probabilities[active_player] *= action_probability;

                // recursively call cfr method, next player to act is the opponent
                cout<<history+Actions[i]<<endl;
                counterfactual_values[i] = -cfr(cards, history+Actions[i], new_reach_probabilities, opponent);

            }

            // // Value of the current game state is just counterfactual values weighted by action probabilities
            float node_value =0.0; // dot product of counterfactual_values and strategy
            for(int i=0;i<strategy.size();i++){
                node_value+=counterfactual_values[i]*strategy[i];
            }
            for(int i=0; i< Actions.size(); i++) {
                info_set.cumulative_regrets[i] += reach_probabilities[opponent] * (counterfactual_values[i] - node_value);
            }
        
            return node_value;
        }

        vector<string> selectKItems(vector<string> stream, int n, int k)
        {
            int i; // index for elements in stream[]
        
            // reservoir[] is the output array. Initialize
            // it with first k elements from stream[]
            vector<string> reservoir;
            for (i = 0; i < k; i++)
                reservoir.push_back(stream[i]);
        
            // Use a different seed value so that we don't get
            // same result each time we run this program
            srand(time(NULL));
        
            // Iterate from the (k+1)th element to nth element
            for (; i < n; i++)
            {
                // Pick a random index from 0 to i.
                int j = rand() % (i + 1);
        
                // If the randomly picked index is smaller than k,
                // then replace the element present at the index
                // with new element from stream
                if (j < k)
                reservoir[j] = stream[i];
            }
            return reservoir;
        }
        float train(int num_iterations) {
            // cout<<"hi"<<endl;
            float util = 0;
            vector<string> kuhn_cards = {"J","Q", "K"};
            cout<<"hi"<<endl;
            for (int i=0;i<num_iterations;i++) {
                cout<<"hello";
                // vector<string> cards = {"J","Q"};
                vector<string> cards = selectKItems(kuhn_cards,kuhn_cards.size(),2);
                cout<<"hello";
                // list1 = [1, 2, 3, 4, 5]  print(sample(list1,3)) => [2, 3, 5]
                string history = ""; // confirm data type
                vector<float> reach_probabilities = {1,1};
                util += cfr(cards, history, reach_probabilities, 0);           
            }
            return util;
        }
};

int main() {

    // Set num_iteration 
    int num_iterations = 100;
    KuhnCFRTrainer cfr_trainer;
    // cout<<"hello"<<endl;
    //cfr_trainer = KuhnCFRTrainer();
    float util = cfr_trainer.train(num_iterations);

    cout<<"\nRunning Kuhn Poker chance sampling CFR for: "<<num_iterations<<" iterations";
    cout<<"\nExpected average game value (for player 1): "<<setprecision(3)<<-1/18;
    cout<<"\nComputed average game value                 "<<setprecision(3)<<util / (num_iterations*1.0);
    cout<<"\nWe expect the bet frequency for a Jack to be between 0 and 1/3";
    cout<<"\nThe bet frequency of a King should be three times the one for a Jack";

    cout<<"\nHistory   Bet   Pass";
    for(auto itr : cfr_trainer.infoset_map){
        cout<<setprecision(3)<<itr.first<<" ";
        for(auto itr1 : itr.second.get_average_strategy() )cout<<setprecision(3)<<itr1<<" ";
        // cout<<itr.second.get_average_strategy()<<endl;
        cout<<endl;
    }

}
